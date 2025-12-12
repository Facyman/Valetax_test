namespace UseCases;

public record PagedResult<T>(
  IReadOnlyList<T> Items,
  int Skip,
  int Count,
  int TotalCount);
