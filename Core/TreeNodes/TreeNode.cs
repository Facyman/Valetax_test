using Ardalis.SharedKernel;

namespace Core.TreeNodes
{
  public class TreeNode(string name, string treeName, int? parentId = null) : EntityBase, IAggregateRoot
  {
    public int? ParentId { get; private set; } = parentId;
    public TreeNode? Parent { get; set; }
    public string Name { get; private set; } = name;
    public string TreeName { get; private set; } = treeName;
    public ICollection<TreeNode> Children { get; set; } = [];

    public TreeNode UpdateName(string newName)
    {
      if (Name == newName) return this;
      Name = newName;
      return this;
    }
  }
}
