using BankingSystem.Configs;
using BankingSystem.Infrastructure.Persistence.Extensions;
using BankingSystem.Middleware;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Solhigson.Framework.Persistence.EntityModels;
using System.Data;
using System.Reflection;

namespace BankingSystem
{
    public partial class Startup
    {

        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

           // builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            builder.Services.AddSingleton<IDbConnection>(db =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Default");
                var connection = new SqlConnection(connectionString);
                return connection;
            });
            builder.Services.AddCors(options => options.AddPolicy("BankingSystem", policyBuilder =>
            {
                var settings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
                policyBuilder.WithOrigins(settings.CORS_ORIGIN)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
            }));

            builder.Services.AddSwaggerGen(option =>
            {
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //option.IncludeXmlComments(xmlPath);

                option.SwaggerDoc("v1", new OpenApiInfo { Title = "BankingSystem Api provider", Version = "v1" });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                    "Enter 'Bearer'  and then your token in the text input " +
                    "below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            return builder;
        }


        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var logConfig = loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                   .Enrich.FromLogContext()
                   .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day,
                   restrictedToMinimumLevel: LogEventLevel.Information,
                   outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                   shared: true);

                if (!builder.Environment.IsDevelopment())
                {
                    logConfig.WriteTo.Sentry(o =>
                    {
                        o.Environment = hostingContext.HostingEnvironment.EnvironmentName;
                        // Debug and higher are stored as breadcrumbs (default is Information)
                        o.MinimumBreadcrumbLevel = LogEventLevel.Information;
                        // Warning and higher is sent as event (default is Error)
                        o.MinimumEventLevel = LogEventLevel.Error;
                        o.Dsn = hostingContext.Configuration.GetValue<string>("AppSettings:SentryUrl");
                        o.Debug = true; // Debug mode for troubleshooting
                        o.TracesSampleRate = 1.0;
                    });
                }
            });
            return builder;
            
        }

    }
}
