using Mediator;

namespace UseCases.Journals.GetRange;

public record GetRangeJournalQuery(int? Skip = 0, int? Take = Constants.DEFAULT_PAGE_SIZE) : IQuery<PagedResult<JournalDto>>;
