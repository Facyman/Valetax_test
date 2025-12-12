namespace Infrastructure.Data
{
  public class AppRepository<T>(AppDbContext dbContext) :
    RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
  {
  }
}
