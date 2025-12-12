using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Web.Configurations;

public static class MiddlewareConfig
{
  public static async Task<IApplicationBuilder> UseAppMiddlewareAndSeedDatabase(this WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }
    else
    {   
      app.UseDefaultExceptionHandler(); // from FastEndpoints
      app.UseHsts();
    }

    app.UseFastEndpoints();

    app.UseExceptionHandler(_ => { });

    if (app.Environment.IsDevelopment())
    {
      app.UseSwaggerGen();
      app.UseSwagger();
      app.UseSwaggerUI();

      app.MapGet("/", () => Results.Redirect("/swagger"));
    }

    var shouldMigrate = app.Environment.IsDevelopment() || 
                        app.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup");
    
    if (shouldMigrate)
    {
      await MigrateDatabaseAsync(app);
    }

    return app;
  }

  static async Task MigrateDatabaseAsync(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
      logger.LogInformation("Applying database migrations...");
      var context = services.GetRequiredService<AppDbContext>();

      await context.Database.MigrateAsync();
      logger.LogInformation("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred migrating the DB. {exceptionMessage}", ex.Message);
      throw; // Re-throw to make startup fail if migrations fail
    }
  }
}
