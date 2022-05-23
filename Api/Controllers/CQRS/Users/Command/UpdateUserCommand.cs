using Api.Models.Entities.Users;
using Api.Models.ResponseModel;
using Api.Models.ORM;
using AutoMapper;
using MediatR;
using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Api.Models.Structs;
using MongoDB.Driver;

namespace Api.Controllers.CQRS.Users.Command
{
    public class UpdateUserCommand : IRequest<Result<bool>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public class Handler : IRequestHandler<UpdateUserCommand, Result<bool>>
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

            public async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                FilterDefinitionBuilder<User> filter = Builders<User>.Filter;

                var mongoResult = await mongo.FindAsync(
                    filter.Eq(x => x._id, session._id),
                    Builders<User>.Projection.Combine(),
                    cancellationToken: cancellationToken);

                //if (!mongoResult.Successfully)
                //    return await response.BadRequest

                return await response.OK(true);
            }
        }
    }
}