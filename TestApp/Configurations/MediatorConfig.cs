using Ardalis.SharedKernel;
using Core;
using UseCases.TreeNodes.Create;


namespace Web.Configurations;

public static class MediatorConfig
{
  // Should be called from ServiceConfigs.cs, not Program.cs
  public static IServiceCollection AddMediatorSourceGen(this IServiceCollection services,
    Microsoft.Extensions.Logging.ILogger logger)
  {
    logger.LogInformation("Registering Mediator SourceGen and Behaviors");
    services.AddExceptionHandler<CustomExceptionHandler>();
    services.AddMediator(options =>
    {
      options.ServiceLifetime = ServiceLifetime.Scoped;

      options.Assemblies =
      [
        typeof(Journal),
        typeof(CreateTreeNodeCommand),
        typeof(InfrastructureServiceExtensions),
        typeof(MediatorConfig)
      ];

      options.PipelineBehaviors =
      [
        typeof(LoggingBehavior<,>)
      ];
    });

    return services;
  }
}
