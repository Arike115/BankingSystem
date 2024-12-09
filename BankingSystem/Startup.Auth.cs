using Autofac.Core;
using BankingSystem.Configs;
using BankingSystem.Domain.Entities;
using BankingSystem.Infrastructure.Persistence;
using BankingSystem.Infrastructure.Persistence.Extensions;
using BankingSystem.Middleware;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BankingSystem
{
    public static partial class Startup
    {
        public static WebApplicationBuilder ConfigureAuthServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                //to be reconfigured
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Lockout.MaxFailedAccessAttempts = 8;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;

            }).AddEntityFrameworkStores<BankingSystemDbContext>()
         .AddDefaultTokenProviders()
         .AddSignInManager<SignInManager<ApplicationUser>>();

            //builder.Services.Configure<IdentityOptions>(options =>
            //{
            //    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            //    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            //    options.ClaimsIdentity.RoleClaimType = Claims.Role;
            //});

            var authSettings = new Authsettings();
            builder.Configuration.Bind(nameof(Authsettings), authSettings);
            var tokenExpiry = TimeSpan.FromMinutes(authSettings.TokenExpiry);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = authSettings.Issuer,
                ValidAudience = authSettings.Issuer,
                ClockSkew = TimeSpan.Zero // Optional: Reduce clock skew for token validation
            };

            // Register TokenValidationParameters
            builder.Services.AddSingleton(tokenValidationParameters);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authSettings.Issuer,
                    ValidAudience = authSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
                };
            }); ;
            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            //builder.Services.AddAuth(builder.Configuration);
            //builder.Services.AddJWT(builder.Configuration);
            //builder.Services.AddSingleton<ExceptionHandlingMiddleware>();
            builder.Services.AddAutoMapper(typeof(Startup));
            builder.Services.AddMvc();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMvcCore().AddControllersAsServices();
            builder.Services.AddMemoryCache();

            builder.Services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            builder.Services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            })
            .ConfigureApiBehaviorOptions(option => { option.SuppressModelStateInvalidFilter = false; });

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}
