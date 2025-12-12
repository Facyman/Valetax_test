using Core.Exceptions;
using Core.TreeNodes;
using Core.TreeNodes.Specifications;

namespace UseCases.TreeNodes.Rename;

public class RenameTreeNodeHandler(IRepository<TreeNode> _repository)
  : ICommandHandler<RenameTreeNodeCommand>
{
  public async ValueTask<Unit> Handle(RenameTreeNodeCommand command, 
    CancellationToken ct)
  {
    var existingTreeNode = await _repository.GetByIdAsync(command.TreeNodeId, ct);
    if (existingTreeNode == null)
    {
      throw new SecureException($"{nameof(TreeNode)} with {command.TreeNodeId} does not exist!");
    }

    var spec = new TreeNodeByParentIdAndNameSpec(existingTreeNode.ParentId, command.NewName);
    var siblingExists = await _repository.AnyAsync(spec, ct);

    if (siblingExists)
    {
      throw new SecureException("Sibling with that name already exists!");
    }

    existingTreeNode.UpdateName(command.NewName);

    await _repository.UpdateAsync(existingTreeNode, ct);

    return Unit.Value;
  }
}
