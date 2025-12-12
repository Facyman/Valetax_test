using Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggerConfigs();

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

startupLogger.LogInformation("Starting web host");

builder.Services.AddServiceConfigs(startupLogger, builder);

builder.Services.AddSwaggerGen();

builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.DocumentSettings = s =>
                  {
                    s.Title = "Test API";
                    s.Version = "v1";
                    s.Description = "HTTP endpoints for the Test API.";
                  };
                  o.ShortSchemaNames = true;
                });

builder.Services.AddEndpointsApiExplorer();

try
{
  var app = builder.Build();

  await app.UseAppMiddlewareAndSeedDatabase();

  app.Run();
}
catch (Exception ex)
{
  startupLogger.LogCritical($"Critical faulure during load, message: {ex.Message}");
}
