﻿#pragma warning disable
using Api.Entities.Identities;
using Api.ORM;
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

public interface ISession
{
    public int id { get; set; }
    public ObjectId _id { get; set; }
    public string token { get; set; }
    public void SetToken(string token);
    public string CreateToken(TimeSpan timeSpan);
}

public class Session : ISession
{
    public Session(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public int id { get; set; }
    public ObjectId _id { get; set; }
    public string token { get; set; }
    private readonly IConfiguration configuration;

    public void SetToken(string requestToken)
    {
        if (string.IsNullOrEmpty(requestToken) || string.IsNullOrWhiteSpace(requestToken))
            return;

        token = requestToken;

        var handler = new JwtSecurityTokenHandler();
        var tokenS = handler.ReadToken(token) as JwtSecurityToken;

        id = int.Parse(tokenS.Claims.First(x => x.Type == nameof(id)).Value);
        _id = ObjectId.Parse(tokenS.Claims.First(x => x.Type == nameof(_id)).Value);
    }

    public string CreateToken(TimeSpan timeSpan)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new List<Claim>();

        claims.Add(new Claim(nameof(id), id.ToString()));
        claims.Add(new Claim(nameof(_id), _id.ToString()));

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.Add(timeSpan),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public interface IIdentityService
{
    public Task<int> GenerateNewId<T>();
    public Task<int> GenerateNewId(string key);
}

public class IdentityService : IIdentityService
{
    protected readonly IMongoORM mongo;
    public IdentityService(IMongoORM mongoORM)
    {
        mongo = mongoORM;
    }

    public async Task<int> GenerateNewId<T>()
    {
        return await GenerateNewId(typeof(T).Name);
    }

    public async Task<int> GenerateNewId(string key)
    {
        var result = await mongo.FindOneAndUpdateAsync(
            Builders<Identity>.Filter.Eq(x => x.Key, key),
            Builders<Identity>.Update.Inc(x => x.Value, 1),
            isUpsert: true
        );

        return result.Item1.Value;
    }
}
#pragma warning restore