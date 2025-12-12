using Mediator;

namespace UseCases.Trees.Get;

public record GetTreeQuery(string TreeName) : IQuery<TreeDto>;
