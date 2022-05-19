using Api.Models.Entities.Users;
using Api.Models.ResponseModel;
using Api.Models.ResponseModel.Models;
using Api.Models.ORM;
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
using Api.Models.Tools;

namespace Api.Controllers.CQRS.Users.Command
{
    public class CreateUserCommand : IRequest<Result<ResultCreate>>
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }

        public class Handler : IRequestHandler<CreateUserCommand, Result<ResultCreate>>
        {
            protected readonly IResponseFactory response;
            protected readonly IRequestSession session;
            protected readonly IMongoORM mongo;
            protected readonly IIdentityService identity;
            protected readonly IMapper map;
            public Handler(
                IResponseFactory responseFactory,
                IRequestSession currentSession,
                IMongoORM mongoORM,
                IIdentityService identityService,
                IMapper mapper)
            {
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