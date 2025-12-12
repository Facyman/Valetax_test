using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using UseCases.Journals.Create;

namespace Web
{
  public class CustomExceptionHandler(ILogger<CustomExceptionHandler> _logger, IServiceScopeFactory _serviceScopeFactory) : IExceptionHandler
  {
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
      var eventId = Guid.NewGuid();

      var problemDetails = new CustomProblemDetails
      {
        Type = ProblemTypeEnum.Exception,
        Id = eventId,
        Data = new ProblemDetailsData()
        {
          Message = $"Internal server error ID = {eventId}"
        },
      };

      if (exception is SecureException)
      {
        problemDetails.Type = ProblemTypeEnum.SecureException;
        problemDetails.Data = new ProblemDetailsData()
        {
          Message = exception.Message,
        };
      }

      try
      {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(new CreateJournalCommand(exception.Message, exception.StackTrace, eventId), cancellationToken);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Unable to write journal during exception handling, Exception: {ex.Message}");
      }

      httpContext.Response.StatusCode = 500;
      await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
      return true;
    }
  }

  public class CustomProblemDetails()
  {
    public required ProblemTypeEnum Type { get; set; }
    public Guid Id { get; set; }

    public ProblemDetailsData? Data { get; set; }
  }

  public class ProblemDetailsData()
  {
    public string? Message { get; set; }
  }

  public enum ProblemTypeEnum
  {
    Unknown = 0,
    Exception = 1,
    SecureException = 2
  }
}
