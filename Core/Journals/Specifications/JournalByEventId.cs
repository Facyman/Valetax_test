using Ardalis.Specification;

namespace Core.TreeNodes.Specifications;

public class JournalByEventId : Specification<Journal>
{
  public JournalByEventId(Guid eventId) =>
    Query
        .Where(journal => journal.EventId == eventId);
}
