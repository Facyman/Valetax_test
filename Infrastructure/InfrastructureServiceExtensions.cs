using Infrastructure.Data;
using Infrastructure.Data.Queries;
using UseCases.Journals.GetRange;
using UseCases.Trees.Get;

namespace Infrastructure
{
  public static class InfrastructureServiceExtensions
  {

    public static IServiceCollection AddInfrastructureServices(
      this IServiceCollection services,
      ConfigurationManager config,
      ILogger logger)
    {
      string? connectionString = config.GetConnectionString("AppDb");
      Guard.Against.Null(connectionString);

      services.AddDbContext<AppDbContext>((provider, options) =>
      {
        options.UseNpgsql(connectionString);
      });

      services.AddScoped(typeof(IRepository<>), typeof(AppRepository<>))
             .AddScoped(typeof(IReadRepository<>), typeof(AppRepository<>))
             .AddScoped<IGetRangeJournalQueryService, GetRangeJournalQueryService>()
             .AddScoped<IGetFlatTreeQueryService, GetFlatTreeQueryService>();

      return services;
    }
  }
}
