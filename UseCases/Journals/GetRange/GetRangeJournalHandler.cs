using Mediator;

namespace UseCases.Journals.GetRange;

public class GetRangeJournalHandler(IGetRangeJournalQueryService query)
  : IQueryHandler<GetRangeJournalQuery, PagedResult<JournalDto>>
{
  public async ValueTask<PagedResult<JournalDto>> Handle(GetRangeJournalQuery request, CancellationToken cancellationToken)
  {
    return await query.GetRangeAsync(request.Skip ?? 0, request.Take ?? Constants.DEFAULT_PAGE_SIZE);
  }
}
