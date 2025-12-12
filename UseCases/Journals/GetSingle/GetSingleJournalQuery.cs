using Mediator;

namespace UseCases.Journals.Get;

public record GetSingleJournalQuery(Guid EventId) : IQuery<JournalDto>;
