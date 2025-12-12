using Core.Exceptions;
using Core.TreeNodes;
using Core.TreeNodes.Specifications;

namespace UseCases.TreeNodes.Create;

public class CreateTreeNodeHandler(IRepository<TreeNode> _repository)
  : ICommandHandler<CreateTreeNodeCommand>
{
  public async ValueTask<Unit> Handle(CreateTreeNodeCommand command,
    CancellationToken cancellationToken)
  {
    var newTreeNode = new TreeNode(command.NodeName, command.TreeName, command.ParentNodeId);

    if (command.ParentNodeId.HasValue)
    {
      var parent = await _repository.GetByIdAsync(command.ParentNodeId.Value, cancellationToken);

      if (parent == null)
      {
        throw new SecureException("Parent does not exist!");
      }

      if (parent != null && parent.TreeName != command.TreeName)
      {
        throw new SecureException("Parent and a child can not have different tree names!");
      }

      var spec = new TreeNodeByParentIdAndNameSpec(command.ParentNodeId.Value, command.NodeName);
      var siblingExists = await _repository.AnyAsync(spec, cancellationToken);

      if (siblingExists)
      {
        throw new SecureException("Sibling with that name already exists!");
      }
    }

    await _repository.AddAsync(newTreeNode, cancellationToken);

    return Unit.Value;
  }
}
