using Ardalis.Specification;

namespace Core.TreeNodes.Specifications;

public class TreeNodeByParentIdAndNameSpec : Specification<TreeNode>
{
  public TreeNodeByParentIdAndNameSpec(int? parentId, string name) =>
    Query
        .Where(treenode => treenode.ParentId == parentId && treenode.Name.ToLower() == name.ToLower());
}
