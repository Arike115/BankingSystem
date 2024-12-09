using DbUp.Helpers;
using DbUp;
using BankingSystem.Middleware;
using DbUp.Engine.Output;

namespace BankingSystem
{
    public static partial class StartUp
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "Rezumii.JobManager v1");
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSentryTracing(); 
            app.UseCors("banking-cors");
            app.UseRouting();
            app.UseHttpsRedirection();
            var serviceProvider = app.Services;

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            TableMigrationScript(app);
            StoredProcedureMigrationScript(app);

            //WebHelpers.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

            return app;
        }


        public static void TableMigrationScript(this WebApplication app)
        {
            string dbConnStr = app.Configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);
            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Tables"))
            .WithTransactionPerScript()
            .JournalToSqlTable("dbo", "TableMigration")
            .LogTo(new SerilogDbUpLog(app.Logger))
            .LogToConsole()
            .Build();
            upgrader.PerformUpgrade();
        }

        /// <summary>
        /// Sql migration for stored procedure
        /// </summary>
        public static void StoredProcedureMigrationScript(this WebApplication app)
        {
            string dbConnStr = app.Configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);
            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Sprocs"))
            .WithTransactionPerScript()
            .JournalTo(new NullJournal())
            .JournalToSqlTable("dbo", "SprocsMigration")
            .LogTo(new SerilogDbUpLog(app.Logger))
            .LogToConsole()
            .Build();

            upgrader.PerformUpgrade();
        }
    }

    public class SerilogDbUpLog : IUpgradeLog
    {
        private readonly ILogger _logger;

        public SerilogDbUpLog(Microsoft.Extensions.Logging.ILogger logger)
        {
            _logger = logger;
        }

        public void WriteError(string format, params object[] args)
        {
            _logger.LogError(format, args);
        }

        public void WriteInformation(string format, params object[] args)
        {
            _logger.LogInformation(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logger.LogWarning(format, args);
        }
    }
}
    

