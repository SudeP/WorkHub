using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Api.ASPNET
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        }

        private const bool enableSwagger = true;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region Service Customaztions

            services.AddValidationCustomaztion();

            if (enableSwagger)
                services.AddSwaggerCustomaztion();

            services.AddApiRegistrationCustomaztion(Configuration);

            services.AddApplicationRegistrationCustomaztion();

            services.AddDependenciesCustomaztion();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
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
            app.Use(OnHandleRequestDelegate);
            app.Use(OnHandleHttpContext);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private RequestDelegate OnHandleRequestDelegate(RequestDelegate requestDelegate)
        {
            return requestDelegate;
        }

        private async Task OnHandleHttpContext(HttpContext context, Func<Task> next)
        {
            ISession session = context.GetSession();

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
