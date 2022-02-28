#pragma warning disable

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

public interface ISession
{
    public int userId { get; set; }
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

    public int userId { get; set; }
    public string token { get; set; }
    private readonly IConfiguration configuration;

    public void SetToken(string requestToken)
    {
        if (string.IsNullOrEmpty(requestToken) || string.IsNullOrWhiteSpace(requestToken))
            return;

        token = requestToken;

        var handler = new JwtSecurityTokenHandler();
        var tokenS = handler.ReadToken(token) as JwtSecurityToken;

        userId = int.Parse(tokenS.Claims.First(x => x.Type == nameof(userId)).Value);
    }

    public string CreateToken(TimeSpan timeSpan)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new List<Claim>();

        claims.Add(new Claim(nameof(userId), userId.ToString()));

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
#pragma warning restore