﻿using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.ORM
{
    public interface IMongoORM
    {
#pragma warning disable
        public IMongoClient client { get; set; }
        public IMongoDatabase db { get; set; }
#pragma warning restore
        public Task<bool> InsertOneAsync<Entity>(
            Entity entity,
            bool? bypassDocumentValidation = null,
            CancellationToken cancellationToken = default);
        public Task<(IEnumerable<Entity>, bool)> FindAsync<Entity>(
            FilterDefinition<Entity> filter,
            Collation collation = null,
            int? limit = null,
            int? skip = null,
            SortDefinition<Entity> sort = null,
            CancellationToken cancellationToken = default);
        public Task<(UpdateResult, bool)> UpdateOneAsync<Entity>(
            FilterDefinition<Entity> filter,
            UpdateDefinition<Entity> updateDefinition,
            Collation collation = null,
            bool? bypassDocumentValidation = null,
            CancellationToken cancellationToken = default);
        public Task<(DeleteResult, bool)> Remove<Entity>(
            FilterDefinition<Entity> filter,
            Collation collation = null,
            CancellationToken cancellationToken = default);
    }
    public class MongoORM : IMongoORM
    {
        public MongoORM(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:Mongo:Default:ConnectionString"];
            databaseName = configuration["ConnectionStrings:Mongo:Default:DataBase"];

            client = new MongoClient(connectionString);

            db = client.GetDatabase(databaseName);
        }

#pragma warning disable
        public IMongoClient client { get; set; }
        public IMongoDatabase db { get; set; }
#pragma warning restore
        public string connectionString;
        public string databaseName;

        public async Task<bool> InsertOneAsync<Entity>(
            Entity entity,
            bool? bypassDocumentValidation = null,
            CancellationToken cancellationToken = default)
        {
            var collection = db.GetCollection<Entity>(typeof(Entity).Name);

            Task task = collection.InsertOneAsync(entity, new InsertOneOptions
            {
                BypassDocumentValidation = bypassDocumentValidation
            }, cancellationToken);

            await task;

            return (task.IsCompletedSuccessfully);
        }

        public async Task<(IEnumerable<Entity>, bool)> FindAsync<Entity>(
            FilterDefinition<Entity> filter,
            Collation collation = null,
            int? limit = null,
            int? skip = null,
            SortDefinition<Entity> sort = null,
            CancellationToken cancellationToken = default)
        {
            var collection = db.GetCollection<Entity>(typeof(Entity).Name);

            Task<IAsyncCursor<Entity>> task = collection.FindAsync(filter, new FindOptions<Entity, Entity>
            {
                Collation = collation,
                Limit = limit,
                Skip = skip,
                Sort = sort
            }, cancellationToken);

            await task;

            return (task.Result.ToList(cancellationToken), task.IsCompletedSuccessfully);
        }

        public async Task<(UpdateResult, bool)> UpdateOneAsync<Entity>(
            FilterDefinition<Entity> filter,
            UpdateDefinition<Entity> updateDefinition,
            Collation collation = null,
            bool? bypassDocumentValidation = null,
            CancellationToken cancellationToken = default)
        {
            var collection = db.GetCollection<Entity>(typeof(Entity).Name);

            Task<UpdateResult> task = collection.UpdateOneAsync(filter, updateDefinition, new UpdateOptions
            {
                BypassDocumentValidation = bypassDocumentValidation,
                Collation = collation,
                IsUpsert = false
            }, cancellationToken);

            await task;

            return (task.Result, task.IsCompletedSuccessfully);
        }

        public async Task<(DeleteResult, bool)> Remove<Entity>(
            FilterDefinition<Entity> filter,
            Collation collation = null,
            CancellationToken cancellationToken = default)
        {
            var collection = db.GetCollection<Entity>(typeof(Entity).Name);

            Task<DeleteResult> task = collection.DeleteOneAsync(filter, new DeleteOptions
            {
                Collation = collation
            }, cancellationToken);

            await task;

            return (task.Result, task.IsCompletedSuccessfully);
        }
    }
}