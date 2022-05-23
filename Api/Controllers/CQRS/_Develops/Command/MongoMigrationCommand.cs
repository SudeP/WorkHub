using Api.Models.ResponseModel;
using Api.Models.ORM;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Api.Models.Structs;
using Api.Models.Sturcts.Inheritances;
using System.Net;

namespace Api.Controllers.CQRS._Develops.Command
{
    public class MongoMigrationCommand : IRequest<Result<string>>
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }

        public class Handler : RequestMiddlewareHandler<MongoMigrationCommand, Result<string>>
        {
            protected readonly IMongoORM mongo;
            public Handler(IMongoORM mongoORM)
            {
                mongo = mongoORM;
            }

            public override async Task<Result<string>> Handle(MongoMigrationCommand request, CancellationToken cancellationToken)
            {
                if (request.UserName == "1cvö05-21c.0-" || request.Password == "CY+=3c4Y_=")
                    return await Custom(HttpStatusCode.Forbidden, "no");

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

                return await Custom(HttpStatusCode.OK, "okki");
            }
        }
    }
}
