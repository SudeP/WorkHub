using Api.Properties.ASPNET;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Api.Models.Tools;

bool enableSwagger = true;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    services.AddControllers();

    if (enableSwagger)
        services.AddSwaggerCustomaztion();

    services.AddApiRegistrationCustomaztion(context.Configuration);

    services.AddApplicationRegistrationCustomaztion();

    services.AddDependenciesCustomaztion();
});

var host = builder.Build();

var app = host.Services.GetService<IApplicationBuilder>();
var env = host.Services.GetService<IWebHostEnvironment>();
var log = host.Services.GetService<ILoggerFactory>();

if (env.EnvironmentName == "PreProduction")
{
    app.UseDeveloperExceptionPage();
}

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
}

log.AddSerilog();

app.UseRouting();

app.UseCors("DevCorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

//middleware
app.Use(OnHandleHttpContext);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

host.Run();

static async Task OnHandleHttpContext(HttpContext context, Func<Task> next)
{
    IRequestSession session = context.GetSession();

    string auth = context.Request.Headers["Authorization"];

    if (!string.IsNullOrEmpty(auth) && !string.IsNullOrWhiteSpace(auth))
    {
        auth = auth.Replace("Bearer", "").Trim();

        if (auth.Length > 0)
            session.SetToken(auth);
    }

    await next();
}