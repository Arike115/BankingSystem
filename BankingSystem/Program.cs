using BankingSystem;
using BankingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args)
    .RegisterServices()
    .RegisterDI()
    .ConfigureSerilog();
builder.WebHost.UseSentry(options =>
{
    options.Dsn = "https://89b221040692e2143821ab6d16152de3@o4507407409545216.ingest.us.sentry.io/45074074537164807416360960";
    options.Debug = true; // Debug mode for troubleshooting
    options.TracesSampleRate = 1.0; // 100% performance traces
});
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables();
builder.ConfigureAuthServices();

var app = builder.Build();

app.ConfigureMiddleware();
app.Run();