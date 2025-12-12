namespace UseCases.Journals.GetRange;

public interface IGetRangeJournalQueryService
{
  Task<PagedResult<JournalDto>> GetRangeAsync(int skip, int take);
}
