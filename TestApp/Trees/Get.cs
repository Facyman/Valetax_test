using FluentValidation;
using System.ComponentModel.DataAnnotations;
using UseCases.Trees;
using UseCases.Trees.Get;
using Web.TreeNodes;

namespace Web.Trees;

public class Get(IMediator mediator) : Endpoint<GetTreeRequest, TreeDto>
{
  public override void Configure()
  {
    Get("api.user.tree.get");
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Get a tree";
      s.Responses[200] = "Success";
      s.Responses[500] = "Error";
    });

    Tags("Trees");

    Description(builder => builder
      .Accepts<GetTreeRequest>()
      .Produces<TreeDto>(200, "application/json")
      .ProducesProblem(500));
  }

  public override async Task HandleAsync(GetTreeRequest req, CancellationToken ct)
  {
    var result = await mediator.Send(new GetTreeQuery(req.TreeName), ct);

    await Send.OkAsync(result, ct);
  }
}

public class GetTreeRequest
{
  [Required]
  public required string TreeName { get; set; }
}

public class GetTreeRequestValidator : Validator<GetTreeRequest>
{
  public GetTreeRequestValidator()
  {
    RuleFor(x => x.TreeName)
        .NotEmpty().WithMessage("TreeName is required");
  }
}

