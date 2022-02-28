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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiRegistration(Configuration);

            services.AddApplicationRegistration(Configuration);

            services.AddDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            if (env.EnvironmentName == "PreProduction")
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.EnvironmentName == "PreProduction")
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

            session.SetToken(
                context.Request.Headers["Authorization"] != ""
                ? context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                : ""
            );

            await next();
        }
    }
}
