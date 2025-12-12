using UseCases;
using UseCases.Journals;
using UseCases.Journals.GetRange;

namespace Infrastructure.Data.Queries;

public class GetRangeJournalQueryService(AppDbContext db) : IGetRangeJournalQueryService
{
  public async Task<PagedResult<JournalDto>> GetRangeAsync(int skip, int take)
  {
    var query = @"
      SELECT
        ""Id"",
        ""EventId"",
        ""CreatedAt"",
        ""Text"",
        ""StackTrace""
      FROM public.""Journals""";

    var items = await db.Journals.FromSqlRaw(query)
      .Skip(skip)
      .Take(take)
      .Select(c => new JournalDto(c.Id, c.EventId, c.CreatedAt, c.Text ?? string.Empty, c.StackTrace ?? string.Empty))
      .AsNoTracking()
      .ToListAsync();

    int count = items.Count;
    int totalCount = db.Journals.Count();
    var result = new PagedResult<JournalDto>(items, skip, count, totalCount);

    return result;
  }
}
