using Api.Models.Entities.Users;
using Api.Models.ResponseModel;
using Api.Models.ORM;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Models.Structs;
using System.Net;
using Api.Models.Sturcts.Inheritances;

namespace Api.Controllers.CQRS.Users.Command
{
    public class SignInUserQuery : IRequest<Result<string>>
    {
        public string Email { get; set; }
        [Required] public string Password { get; set; }

        public class Handler : RequestMiddlewareHandler<SignInUserQuery, Result<string>>
        {
            protected readonly HttpContext context;
            protected readonly IRequestSession session;
            protected readonly IMongoORM mongo;
            protected readonly IMapper map;
            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IRequestSession currentSession,
                IMongoORM mongoORM,
                IMapper mapper)
            {
                context = httpContextAccessor.HttpContext;
                session = currentSession;
                mongo = mongoORM;
                map = mapper;
            }

            public override async Task<Result<string>> Handle(SignInUserQuery request, CancellationToken cancellationToken)
            {
                var filter = Builders<User>.Filter;

                var task = await mongo.FindAsync(
                    filter.And(
                        filter.Eq(x => x.Email, request.Email),
                        filter.Eq(x => x.Password, request.Password),
                        filter.Eq(x => x.IsActive, true),
                        filter.Eq(x => x.IsDelete, false)
                    ),
                    Builders<User>.Projection.Combine(),
                    cancellationToken: cancellationToken
                );

                var entity = task.Entity.FirstOrDefault();

                if (Equals(entity, null))
                    return await Custom(HttpStatusCode.NotFound, string.Empty);

                session.Identity = entity.Identity;
                session._id = entity._id;

                string createdToken = session.CreateToken(TimeSpan.FromDays(1));

                context.Response.Headers.Remove("Authorization");
                context.Response.Headers.Add("Authorization", createdToken);
                context.Response.Headers.Remove("Access-Control-Expose-Headers");
                context.Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");

                return await Custom(HttpStatusCode.Created, createdToken);
            }
        }
    }
}