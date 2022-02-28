using Api.Entities.Users;
using Api.Models.ResponseModel;
using Api.ORM;
using Api.Validation.Users;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Api.CQRS.Users.Command
{
    public class SignUpUserCommand : IRequest<Result<object>>
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string GivenName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Phone { get; set; }

        public class Handler : IRequestHandler<SignUpUserCommand, Result<object>>
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

            public async Task<Result<object>> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
            {
                User entity = map.Map<User>(request);

                entity._id = ObjectId.GenerateNewId();
                entity.IsActive = true;
                entity.IsDelete = false;
                entity.CreateDate = DateTime.Now;
                entity.CreateBy = session.userId;

                var successfully = await mongo.InsertOneAsync(entity, cancellationToken: cancellationToken);

                var result = successfully ? response.Created<object>() : response.InternalServerError<object>();

                return await Task.FromResult(result).ConfigureAwait(false);
            }
        }
    }
}
