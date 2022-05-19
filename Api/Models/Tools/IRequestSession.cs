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

namespace Api.Models.Tools
{
    public interface IRequestSession
    {
        public int Identity { get; set; }
        public string _id { get; set; }
        public string token { get; set; }
        public void SetToken(string token);
        public string CreateToken(TimeSpan timeSpan);
    }
}
#pragma warning restore