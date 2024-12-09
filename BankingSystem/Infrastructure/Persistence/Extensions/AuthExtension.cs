using BankingSystem.Configs;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BankingSystem.Infrastructure.Persistence.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = new JwtSettings
            {
                SigningKey = Configuration["Jwt:SigningKey"],
                ExpiryInMinutes = int.Parse(Configuration["Jwt:ExpiryInMinutes"]),
                Issuer = Configuration["Jwt:Issuer"]
            };

            services.AddSingleton<JwtSettings>(jwtSettings);

            return services;
        }

        public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration)
        {

            var authenticationProviderKey = "Bearer";
            var signingKey = configuration.GetSection("Jwt:SigningKey").Value;
            var issuer = configuration.GetSection("Jwt:Issuer").Value;

            var jwtSettings = new JwtSettings
            {
                SigningKey = configuration["Jwt:SigningKey"],
                ExpiryInMinutes = int.Parse(configuration["Jwt:ExpiryInMinutes"]),
                Issuer = configuration["Jwt:Issuer"]
            };

            services.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = authenticationProviderKey;
                options.DefaultScheme = authenticationProviderKey;
                options.DefaultChallengeScheme = authenticationProviderKey;
                options.DefaultForbidScheme = authenticationProviderKey;
            })
                .AddJwtBearer(authenticationProviderKey, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            return services;

        }
    }
}
