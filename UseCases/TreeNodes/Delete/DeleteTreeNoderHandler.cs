using Core.Exceptions;
using Core.TreeNodes;

namespace UseCases.TreeNodes.Delete;

public class DeleteTreeNodeHandler(IRepository<TreeNode> repository)
  : ICommandHandler<DeleteTreeNodeCommand>
{
  public async ValueTask<Unit> Handle(DeleteTreeNodeCommand request, CancellationToken cancellationToken)
  {
    var aggregateToDelete = await repository.GetByIdAsync(request.Id, cancellationToken);
    if (aggregateToDelete == null) throw new SecureException($"{nameof(TreeNode)} with {request.Id} does not exist!");
    await repository.DeleteAsync(aggregateToDelete, cancellationToken);

    return Unit.Value;
  }
}
