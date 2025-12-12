using FluentValidation;
using UseCases.Journals;
using UseCases.Journals.Get;
using Web.TreeNodes;

namespace Web.Journals;

public class GetSingle(IMediator mediator) : Endpoint<GetSingleJournalRequest, JournalDto>
{
  public override void Configure()
  {
    Get("api.user.journal.getSingle");
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Get a journal";
      s.Responses[200] = "Success";
      s.Responses[500] = "Error";
    });

    Tags("Journals");

    Description(builder => builder
      .Accepts<GetSingleJournalRequest>()
      .Produces<JournalDto>(200, "application/json")
      .ProducesProblem(500));
  }

  public override async Task HandleAsync(GetSingleJournalRequest req, CancellationToken cancellationToken)
  {
    var response = await mediator.Send(new GetSingleJournalQuery(req.EventId!.Value), cancellationToken);
    await Send.OkAsync(response, cancellationToken);
  }
}

public class GetSingleJournalRequest
{
  [BindFrom("id")]
  public Guid? EventId { get; set; }
}

public class GetSingleJournalRequestValidator : Validator<GetSingleJournalRequest>
{
  public GetSingleJournalRequestValidator()
  {
    RuleFor(x => x.EventId)
        .NotEmpty().WithMessage("EventId is required");
  }
}
