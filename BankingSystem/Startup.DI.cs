using BankingSystem.Infrastructure.Persistence;
using BankingSystem.Interface;
using BankingSystem.Services;
using BankingSystem.Services.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace BankingSystem
{
    public partial class Startup
    {

            public static WebApplicationBuilder RegisterDI(this WebApplicationBuilder builder)
            {
                builder.Services.AddControllers();

                var redisMultiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
                builder.Services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);


                builder.Services.AddSingleton<ICacheService, CacheService>();


            builder.Services.AddDbContext<BankingSystemDbContext>(options =>
                {
                    var connectionstring = builder.Configuration.GetConnectionString("Default");

                    options.UseSqlServer(connectionstring);
                });


                // Add services to the container
                //RepositoryRegistration.RepositoryRegDI(builder);
                builder.Services.AddTransient<IAccountRepository, AccountRepository>();
                builder.Services.AddTransient<IJwtHandler, JwtHandler>();
                //builder.Services.AddTransient<IAuthorizationHandler, PermissionsAuthorizationHandler>();
                //builder.Services.AddTransient<IBusinessService, BusinessService>();
                //builder.Services.AddTransient<IBusinessCategoryService, BusinessCategoryService>();



                return builder;

            }

        
    }
}
