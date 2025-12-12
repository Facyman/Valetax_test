namespace UseCases.TreeNodes.Create;

public record CreateTreeNodeCommand(string TreeName, string NodeName, int? ParentNodeId) : ICommand;
