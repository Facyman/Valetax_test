using Core;

namespace UseCases.Journals.Create;

public class CreateJournalHandler(IRepository<Journal> _repository)
  : ICommandHandler<CreateJournalCommand, JournalDto>
{
  public async ValueTask<JournalDto> Handle(CreateJournalCommand command,
    CancellationToken cancellationToken)
  {
    var newJournal = new Journal(Guid.NewGuid(), command.Text, command.StackTrace);

    var res = await _repository.AddAsync(newJournal, cancellationToken);

    return new JournalDto(res.Id, res.EventId, res.CreatedAt, res.Text ?? string.Empty, res.StackTrace ?? string.Empty);
  }
}
