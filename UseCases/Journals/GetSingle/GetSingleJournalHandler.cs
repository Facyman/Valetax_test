using Core;
using Core.Exceptions;
using Core.TreeNodes.Specifications;
using UseCases.Journals.Get;

namespace UseCases.Journals.GetSingle;

public class GetSingleJournalHandler(IReadRepository<Journal> _repository)
  : IQueryHandler<GetSingleJournalQuery, JournalDto>
{
  public async ValueTask<JournalDto> Handle(GetSingleJournalQuery req, CancellationToken cancellationToken)
  {
    var spec = new JournalByEventId(req.EventId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) throw new SecureException($"{nameof(Journal)} with Event ID: {req.EventId} does not exist!");

    return new JournalDto(entity.Id, entity.EventId, entity.CreatedAt, entity.Text ?? string.Empty, entity.StackTrace ?? string.Empty);
  }
}
