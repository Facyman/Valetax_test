namespace UseCases.Journals;
public record JournalDto(int Id, Guid? EventId, DateTime CreatedAt, string Text, string StackTrace);
