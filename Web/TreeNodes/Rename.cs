using FluentValidation;
using System.ComponentModel.DataAnnotations;
using UseCases.TreeNodes.Rename;

namespace Web.TreeNodes;

public class Rename(IMediator mediator) : Endpoint<RenameTreeNodeRequest>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post("api.user.tree.node.rename");
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Rename a treenode";
      s.Responses[204] = "Success";
      s.Responses[500] = "Error";
    });

    Tags("TreeNodes");

    Description(builder => builder
      .Accepts<RenameTreeNodeRequest>("application/json")
      .Produces(204)
      .ProducesProblem(500));
  }

  public override async Task HandleAsync(RenameTreeNodeRequest request, CancellationToken ct)
  {
    var cmd = new RenameTreeNodeCommand(
      request.Id!.Value,
      request.NewNodeName!);

    await _mediator.Send(cmd, ct);

    await Send.NoContentAsync(ct);
  }
}

public class RenameTreeNodeRequest
{
  [Required]
  public int? Id { get; set; }
  [Required]
  public string? NewNodeName { get; set; }
}

public class RenameTreeNodeRequestValidator : Validator<RenameTreeNodeRequest>
{
  public RenameTreeNodeRequestValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id is required");

    RuleFor(x => x.NewNodeName)
    .NotEmpty().WithMessage("NewNodeName is required");
  }
}
