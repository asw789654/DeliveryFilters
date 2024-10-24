using Core.Api;
using Core.Api.Middlewares;
using Core.Application;
using Infrastructure.Persistence;
using Orders.Application;
using Serilog;
using Serilog.Events;
using System.Reflection;

try
{
    const string version = "v1";
    const string appName = "Orders API v1";

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
#if DEBUG
        .WriteTo.Console()
#endif
        .WriteTo.File($"{builder.Configuration["Logging:LogsFolder"]}/Information-.txt", LogEventLevel.Information,
            rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3, buffered: true)
        .WriteTo.File($"{builder.Configuration["Logging:LogsFolder"]}/Warning-.txt", LogEventLevel.Warning,
            rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14, buffered: true)
        .WriteTo.File($"{builder.Configuration["Logging:LogsFolder"]}/Error-.txt", LogEventLevel.Error,
            rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30, buffered: true));

    builder.Services
        .AddCoreApiServices()
        .AddCoreApplicationServices()
        .AddPersistenceServices(builder.Configuration)
        .AddAllCors()
        .AddOrdersApplication()
        .AddSwagger(Assembly.GetExecutingAssembly(), appName, version, appName);

    var app = builder.Build();

    app.RunDbMigrations().RegisterApis(Assembly.GetExecutingAssembly(), $"api/{version}");

    app.UseCoreExceptionHandler() 
        .UseSwagger(c => { c.RouteTemplate = "swagger/{documentname}/swagger.json"; })
        .UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
            options.RoutePrefix = "swagger";
        })
        .UseHttpsRedirection()
        .UseCors();

    app.Run();
}
catch (Exception ex)
{
    var appSettingsFile = $"{Directory.GetCurrentDirectory()}/appsettings.json";
    var settingsJson = File.ReadAllText(appSettingsFile);
    var appSettings = System.Text.Json.JsonDocument.Parse(settingsJson);
    var logsPath = appSettings.RootElement.GetProperty("Logging").GetProperty("LogsFolder").GetString();
    var logger = new LoggerConfiguration()
        .WriteTo.File($"{logsPath}/Log-Run-Error-.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Hour,
            retainedFileCountLimit: 30)
        .CreateLogger();
    logger.Fatal(ex.Message, ex);
}