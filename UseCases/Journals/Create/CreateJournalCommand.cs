namespace UseCases.Journals.Create;

public record CreateJournalCommand(string Text, string? StackTrace = null, Guid? EventId = null) : ICommand<JournalDto>;
