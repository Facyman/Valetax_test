using FluentValidation;
using Mediator;
using System.ComponentModel.DataAnnotations;
using UseCases.TreeNodes.Create;

namespace Web.TreeNodes;

public class Create(IMediator mediator)
  : Endpoint<CreateTreeNodeRequest>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post("api.user.tree.node.create");
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Create a new contributor";
      s.Responses[204] = "Success";
      s.Responses[500] = "Error";
    });

    Tags("TreeNodes");

    Description(builder => builder
      .Accepts<CreateTreeNodeRequest>("application/json")
      .Produces(204)
      .ProducesProblem(500));
  }

  public override async Task HandleAsync(CreateTreeNodeRequest req, CancellationToken ct)
  {
    await _mediator.Send(new CreateTreeNodeCommand(req.TreeName!, req.NodeName!, req.ParentId), ct);

    await Send.NoContentAsync(ct);
  }
}

public class CreateTreeNodeRequest()
{
  [Required]
  public string? TreeName { get; set; }
  [Required]
  public string? NodeName { get; set; }
  public int? ParentId { get; set; }
}

public class CreateTreeNodeRequestValidator : Validator<CreateTreeNodeRequest>
{
  public CreateTreeNodeRequestValidator()
  {
    RuleFor(x => x.TreeName)
        .NotEmpty().WithMessage("TreeName is required");

    RuleFor(x => x.NodeName)
        .NotEmpty().WithMessage("NodeName is required");
  }
}
