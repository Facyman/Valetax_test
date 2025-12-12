using FluentValidation;
using System.ComponentModel.DataAnnotations;
using UseCases.TreeNodes.Delete;

namespace Web.TreeNodes;

public class Delete(IMediator _mediator)
  : Endpoint<DeleteContributorRequest>
{
  public override void Configure()
  {
    Delete("api.user.tree.node.delete");
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Delete a treenode";
      s.Responses[204] = "Success";
      s.Responses[500] = "Error";
    });

    Tags("TreeNodes");

    Description(builder => builder
      .Accepts<DeleteContributorRequest>()
      .Produces(204)
      .ProducesProblem(500));
  }

  public override async Task HandleAsync(DeleteContributorRequest req, CancellationToken ct)
  {
    var cmd = new DeleteTreeNodeCommand(req.NodeId.Value);
    await _mediator.Send(cmd, ct);

    await Send.NoContentAsync(ct);
  }
}

public record DeleteContributorRequest
{
  [Required]
  public int? NodeId { get; set; }
}

public class DeleteContributorRequestValidator : Validator<DeleteContributorRequest>
{
  public DeleteContributorRequestValidator()
  {
    RuleFor(x => x.NodeId)
        .NotEmpty().WithMessage("NodeId is required");
  }
}

