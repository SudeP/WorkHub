#pragma warning disable
using Api.Models.Entities.Identities;
using Api.Models.ORM;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Models.Structs
{
    public class IdentityService : IIdentityService
    {
        protected readonly IMongoORM mongo;
        public IdentityService(IMongoORM mongoORM)
        {
            mongo = mongoORM;
        }

        public async Task<int> GenerateNewIdentity<T>()
        {
            return await GenerateNewIdentity(typeof(T).Name);
        }

        public async Task<int> GenerateNewIdentity(string key)
        {
            var result = await mongo.FindOneAndUpdateAsync(
                Builders<Identity>.Filter.Eq(x => x.Key, key),
                Builders<Identity>.Update.Inc(x => x.Value, 1),
                Builders<Identity>.Projection.Combine(),
                isUpsert: true
            );

            return result.Item1.Value;
        }
    }
}
#pragma warning restore