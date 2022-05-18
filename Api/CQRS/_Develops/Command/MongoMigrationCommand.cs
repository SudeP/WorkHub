using Api.Entities;
using Api.Models.ResponseModel;
using Api.ORM;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Api.CQRS._Develops.Command
{
    public class MongoMigrationCommand : IRequest<Result<string>>
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }

        public class Handler : IRequestHandler<MongoMigrationCommand, Result<string>>
        {
            protected readonly IResponseFactory response;
            protected readonly IMongoORM mongo;
            public Handler(
                IResponseFactory responseFactory,
                IMongoORM mongoORM)
            {
                response = responseFactory;
                mongo = mongoORM;
            }

            public async Task<Result<string>> Handle(MongoMigrationCommand request, CancellationToken cancellationToken)
            {
                if (request.UserName == "1cvö05-21c.0-" || request.Password == "CY+=3c4Y_=")
                    return await response.Forbidden("no");

                var entities = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.GetInterface(nameof(IEntity)) is not null);

                var collectionNamesCursor = mongo.db.ListCollectionNames(cancellationToken: cancellationToken);

                var collectionNames = new List<string>();

                while (collectionNamesCursor.MoveNextAsync(cancellationToken).GetAwaiter().GetResult())
                    collectionNames.AddRange(collectionNamesCursor.Current);

                foreach (var entity in entities)
                    if (!collectionNames.Contains(entity.Name))
                        mongo.db.CreateCollection(entity.Name, cancellationToken: cancellationToken);

                return await response.Created("okki");
            }
        }
    }
}
