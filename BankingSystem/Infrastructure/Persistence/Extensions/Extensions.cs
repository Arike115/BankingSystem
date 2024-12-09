using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using FluentValidation.AspNetCore;
using BankingSystem.Application.Commands.AccountRegistration;
using Microsoft.OpenApi.Models;

namespace BankingSystem.Infrastructure.Persistence.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");

            services.AddDbContext<BankingSystemDbContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });//.AddUnitOfWork<BankingSystemDbContext>();

            services.AddTransient(sp => CreateConnection(configuration));
            return services;
        }

        public static IDbConnection CreateConnection(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            return new SqlConnection(connectionString);
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            #region Validators

            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<AccountCommandCommandValidator>();
            });

            #endregion

            return services;
        }

        public static IServiceCollection AddSwaggerService(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>

            {
                //c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Banking Sytem", Version = "1.0" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "Banking System.xml");
                c.IncludeXmlComments(filePath: filePath, true);
            });
            return services;
        }
    }
}
