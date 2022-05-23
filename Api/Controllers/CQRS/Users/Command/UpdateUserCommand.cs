using Api.Models.Entities.Users;
using Api.Models.ResponseModel;
using Api.Models.ORM;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Api.Models.Structs;
using MongoDB.Driver;
using Api.Models.ResponseModel.Models;
using Api.Models.Sturcts.Inheritances;
using Newtonsoft.Json;

namespace Api.Controllers.CQRS.Users.Command
{
    public class UpdateUserCommand : IRequest<Result<ResultUpdate>>
    {
        [JsonIgnore] public IRequestSession Session { get; set; }
        [JsonIgnore] public IMongoORM Mongo { get; set; }
        public string UserName { get; set; }

        public class Handler : RequestMiddlewareHandler<UpdateUserCommand, Result<ResultUpdate>>
        {
            public override async Task<Result<ResultUpdate>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var entity = await request.Mongo.FindOneAndUpdateAsync(
                   Builders<User>.Filter.Eq(x => x._id, request.Session._id),
                   Builders<User>.Update.Set(x => x.UserName, request.UserName),
                   Builders<User>.Projection.Combine(),
                   cancellationToken: cancellationToken);

                return null;
            }
        }
    }
}