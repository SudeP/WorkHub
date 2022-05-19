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
    public interface IIdentityService
    {
        public Task<int> GenerateNewIdentity<T>();
        public Task<int> GenerateNewIdentity(string key);
    }
}
#pragma warning restore