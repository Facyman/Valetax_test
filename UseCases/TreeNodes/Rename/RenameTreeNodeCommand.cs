using UseCases.Trees;

namespace UseCases.TreeNodes.Rename;

public record RenameTreeNodeCommand(int TreeNodeId, string NewName) : ICommand;
