using Ardalis.SharedKernel;

namespace Core
{
  public class Journal(Guid eventId, string? text = null, string? stackTrace = null) : EntityBase, IAggregateRoot
  {
    public Guid EventId { get; private set; } = eventId;
    public DateTime CreatedAt { get; set; }
    public string? Text { get; private set; } = text;
    public string? StackTrace { get; private set; } = stackTrace;
  }
}
