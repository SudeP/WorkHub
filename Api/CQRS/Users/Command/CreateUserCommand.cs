using Api.Entities.Users;
using Api.Models.ResponseModel;
using Api.Models.ResponseModel.Models;
using Api.ORM;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Api.CQRS.Users.Command
{
    public class CreateUserCommand : IRequest<Result<ResultCreate>>
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }

        public class Handler : IRequestHandler<CreateUserCommand, Result<ResultCreate>>
        {
            protected readonly HttpContext context;
            protected readonly IResponseFactory response;
            protected readonly ISession session;
            protected readonly IMongoORM mongo;
            protected readonly IIdentityService identity;
            protected readonly IMapper map;
            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IResponseFactory responseFactory,
                ISession currentSession,
                IMongoORM mongoORM,
                IIdentityService identityService,
                IMapper mapper)
            {
                context = httpContextAccessor.HttpContext;
                response = responseFactory;
                session = currentSession;
                mongo = mongoORM;
                identity = identityService;
                map = mapper;
            }

            public async Task<Result<ResultCreate>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                User entity = map.Map<User>(request);

                var filter = Builders<User>.Filter;

                var exists = mongo.FindAsync(
                    filter.Eq(x => x.Email, request.Email),
                    Builders<User>.Projection.Combine(),
                    cancellationToken: cancellationToken);

                if (!Equals(exists, null))
                    return await response.BadRequest<ResultCreate>(responseMessages: new List<ResponseMessage>
                    {
                        new ResponseMessage { Message = "This email exists already" }
                    });

                entity.Identity = await identity.GenerateNewIdentity<User>();
                entity._id = ObjectId.GenerateNewId().ToString();
                entity.IsActive = true;
                entity.IsDelete = false;
                entity.IsApproved = false;
                entity.CreateDate = DateTime.Now;
                entity.CreateBy = session.Identity;

                var successfully = await mongo.InsertOneAsync(entity, cancellationToken: cancellationToken);

                return await (successfully
                    ? response.Created(map.Map<ResultCreate>(entity))
                    : response.InternalServerError<ResultCreate>());
            }
        }
    }
}