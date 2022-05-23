using Api.Models.Structs;
using Api.Properties.ASPNET;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(whb =>
{
    whb.UseStartup<Startup>();
}).Build().Run();

namespace Api.Properties.ASPNET
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            _enableSwagger = !Debugger.IsAttached;
            if (1 == 1) _enableSwagger = true;
        }

        private readonly IConfiguration _configuration;
        readonly bool _enableSwagger;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            if (_enableSwagger)
                services.AddSwaggerCustomaztion();

            services.AddApiRegistrationCustomaztion(_configuration);

            services.AddApplicationRegistrationCustomaztion();

            services.AddDependenciesCustomaztion();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            if (env.EnvironmentName == "PreProduction")
            {
                app.UseDeveloperExceptionPage();
            }

            if (_enableSwagger)
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
        }

        private async Task OnHandleHttpContext(HttpContext context, Func<Task> next)
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
    }
}