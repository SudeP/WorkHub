using Api.Models.ResponseModel;
using Api.Models.ORM;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Serilog;
using Api.Models.Structs;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Api.Properties.ASPNET
{
    public static class ServiceRegistration
    {
        public static void AddSwaggerCustomaztion(this IServiceCollection services)
        {
            //@TODO: Swagger'da API açýklamalarý için daha sonrasýnda buraya entegrasyon yapýlacak.
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WorkHub API",
                    Description = "WorkHub Documentation"
                });

                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                //swagger.EnableAnnotations();
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
            });
        }

        public static void AddApiRegistrationCustomaztion(this IServiceCollection services, IConfiguration Configuration)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(Configuration)
            .CreateLogger();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                };
            });

            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy("DevCorsPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            //NOTE: Custom Exception kullanmak için yazıldı.
            services.AddControllers(options => options.Filters.Add(typeof(ExceptionMiddleware))).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new HelperDateTime.DateTimeConverter());
            });


            //NOTE: Url üzerinde büyük harf falan yazılırsa onları küçük harfe çevirmesi için eklendi.
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        public static void AddDependenciesCustomaztion(this IServiceCollection services)
        {
            services.AddScoped<IRequestSession, RequestSession>();
            services.AddScoped<IMongoORM, MongoORM>();

            services.AddSingleton<IIdentityService, IdentityService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddApplicationRegistrationCustomaztion(this IServiceCollection services)
        {
            var assm = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assm);
            services.AddMediatR(assm);
            services.AddValidatorsFromAssembly(assm);


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }

        public class HelperDateTime
        {
            public class DateTimeConverter : JsonConverter<DateTime>
            {
                public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    return DateTime.Parse(reader.GetString());
                }

                public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
                {
                    writer.WriteStringValue(value.ToUniversalTime().ToLocalTime().ToString("dd-MM-yyyyTHH:mm:ss"));
                }
            }
        }
    }
}
