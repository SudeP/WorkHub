using Api.Entities.Users;
using Api.Models.ResponseModel;
using Api.ORM;
using Api.Validation.Users;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.CQRS.Users.Command
{
    public class SignInUserCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required] public string Password { get; set; }

        public class Handler : IRequestHandler<SignInUserCommand, Result<string>>
        {
            protected readonly HttpContext context;
            protected readonly IResponseFactory response;
            protected readonly ISession session;
            protected readonly IMongoORM mongo;
            protected readonly IMapper map;
            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IResponseFactory responseFactory,
                ISession currentSession,
                IMongoORM mongoORM,
                IMapper mapper)
            {
                context = httpContextAccessor.HttpContext;
                response = responseFactory;
                session = currentSession;
                mongo = mongoORM;
                map = mapper;
            }

            public async Task<Result<string>> Handle(SignInUserCommand request, CancellationToken cancellationToken)
            {
                var filter = Builders<User>.Filter;

                var task = await mongo.FindAsync(
                    filter.And(
                        filter.Eq(x => x.Password, request.Password),
                        filter.Or(
                            filter.Eq(x => x.Email, request.Email),
                            filter.Eq(x => x.UserName, request.UserName)
                        )
                    ),
                    cancellationToken: cancellationToken
                );

                var entity = task.Item1.First();

                session.id = entity.Id;
                session._id = entity._id;

                string createdToken = session.CreateToken(TimeSpan.FromDays(1));

                context.Response.Headers.Remove("Authorization");
                context.Response.Headers.Add("Authorization", createdToken);
                context.Response.Headers.Remove("Access-Control-Expose-Headers");
                context.Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");

                var successfully = await mongo.InsertOneAsync(entity, cancellationToken: cancellationToken);

                var result = successfully ? response.Created(createdToken) : response.InternalServerError<string>();

                return await Task.FromResult(result).ConfigureAwait(false);
            }
        }
    }
}
