using UseCases;
using UseCases.Journals;
using UseCases.Journals.GetRange;

namespace Web.Journals;

public class GetRange(IMediator mediator) : Endpoint<GetRangeJournalsRequest, PagedResult<JournalDto>>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Get("api.user.journal.getRange");
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Range of journals";
      s.ExampleRequest = new GetRangeJournalsRequest { Skip = 0, Take = 10 };
      s.Responses[200] = "Success";
      s.Responses[500] = "Error";
    });

    Tags("Journals");

    Description(builder => builder
      .Accepts<GetRangeJournalsRequest>()
      .Produces<PagedResult<JournalDto>>(200, "application/json")
      .ProducesProblem(400));
  }

  public override async Task HandleAsync(GetRangeJournalsRequest request, CancellationToken cancellationToken)
  {
    var response = await _mediator.Send(new GetRangeJournalQuery(request.Skip, request.Take), cancellationToken);
    await Send.OkAsync(response, cancellationToken);
  }
}

public sealed class GetRangeJournalsRequest
{
  [BindFrom("skip")]
  public int Skip { get; init; } = 0;

  [BindFrom("take")]
  public int Take { get; init; } = Constants.DEFAULT_PAGE_SIZE;
}
