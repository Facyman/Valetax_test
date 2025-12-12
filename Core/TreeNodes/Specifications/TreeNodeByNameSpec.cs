using Ardalis.Specification;

namespace Core.TreeNodes.Specifications;

public class TreeNodeByNameSpec : Specification<TreeNode>
{
  public TreeNodeByNameSpec(string name) =>
    Query
        .Where(treenode => treenode.Name.ToLower() == name.ToLower());
}
