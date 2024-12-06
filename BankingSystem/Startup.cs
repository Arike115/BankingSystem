using BankingSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BankingSystem
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private ILogger<Startup> _logger;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            ConfigureDI(services);
            services.AddPersistence(Configuration);
            services.AddApplicationServices(Configuration);


            services.AddSwaggerService(Configuration);
            //services.AddSingleton<ExceptionHandlingMiddleware>();
            //services.AddAutoMapper(typeof(Startup));
            services.AddMvc();
            services.AddHttpContextAccessor();
            services.AddMvcCore().AddControllersAsServices();
            services.AddMemoryCache();

            //services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
                //options.Filters.Add<ValidationFilter>();
            })
            //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
            .ConfigureApiBehaviorOptions(option => { option.SuppressModelStateInvalidFilter = false; });
            services.AddControllers();

           
            //services.AddHttpClient<SlackService>();
            

            //services.UseSerilog((hostingContext, loggerConfiguration) =>
            //{
            //    var logConfig = loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            //});

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

           // app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (env.IsDevelopment())
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



            //app.UseSentryTracing();

            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            

            var serviceProvider = app.ApplicationServices;
           

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html" }
            });
            app.UseStaticFiles();
            //app.UseSerilogRequestLogging();

            //TableMigrationScript();
            //StoredProcedureMigrationScript();
        }
        //public class SerilogDbUpLog : IUpgradeLog
        //{
        //    private readonly ILogger<Startup> _logger;

        //    public SerilogDbUpLog(ILogger<Startup> logger)
        //    {
        //        _logger = logger;
        //    }

        //    public void WriteError(string format, params object[] args)
        //    {
        //        Log.Error(format, args);
        //    }

        //    public void WriteInformation(string format, params object[] args)
        //    {
        //        Log.Information(format, args);
        //    }

        //    public void WriteWarning(string format, params object[] args)
        //    {
        //        Log.Warning(format, args);
        //    }
        //}


    }
}
