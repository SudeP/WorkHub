﻿using Api.Entities.Users;
using Api.Models.ResponseModel;
using Api.Models.ResponseModel.Models;
using Api.ORM;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Api.CQRS.Users.Command
{
    public class CreateUserCommand : IRequest<Result<ResultCreate>>
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string GivenName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        public string Phone { get; set; }

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

                entity.Id = await identity.GenerateNewId<User>();
                entity._id = ObjectId.GenerateNewId();
                entity.IsActive = true;
                entity.IsDelete = false;
                entity.CreateDate = DateTime.Now;
                entity.CreateBy = session.userId;

                var successfully = await mongo.InsertOneAsync(entity, cancellationToken: cancellationToken);

                var result = successfully
                    ? response.Created(map.Map<ResultCreate>(entity))
                    : response.InternalServerError<ResultCreate>();

                return await Task.FromResult(result).ConfigureAwait(false);
            }
        }
    }
}
