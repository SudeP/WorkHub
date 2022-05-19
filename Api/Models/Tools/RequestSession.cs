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
    public class RequestSession : IRequestSession
    {
        public RequestSession(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public int Identity { get; set; }
        public string _id { get; set; }
        public string token { get; set; }
        private readonly IConfiguration configuration;

        public void SetToken(string requestToken)
        {
            if (string.IsNullOrEmpty(requestToken) || string.IsNullOrWhiteSpace(requestToken))
                return;

            token = requestToken;

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            Identity = int.Parse(tokenS.Claims.First(x => x.Type == nameof(Identity)).Value);
            _id = tokenS.Claims.First(x => x.Type == nameof(_id)).Value;
        }

        public string CreateToken(TimeSpan timeSpan)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(nameof(Identity), Identity.ToString()));
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
}
#pragma warning restore