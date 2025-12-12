using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.TreeNodes;
using Core.TreeNodes.Specifications;

namespace UseCases.Trees.Get;

public class GetTreeHandler(
  IReadRepository<TreeNode> _repository, 
  IGetFlatTreeQueryService flatTreeQuery)
  : IQueryHandler<GetTreeQuery, TreeDto>
{
  public async ValueTask<TreeDto> Handle(GetTreeQuery request, CancellationToken cancellationToken)
  {
    var spec = new TreeNodeByParentIdAndNameSpec(null, request.TreeName);
    var rootNode = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

    if (rootNode == null)
    {
      throw new SecureException($"Tree with {request.TreeName} does not exist!");
    }

    var resultTree = new TreeDto() 
    { 
      Id = rootNode.Id,
      Name = rootNode.Name,
      TreeName = rootNode.Name,
      Children = []
    };

    var flatTree = await flatTreeQuery.GetAsync(rootNode.Id);

    var completeTree = BuildTree(flatTree, rootNode.Id);

    if (completeTree != null)
    {
      resultTree = completeTree;
    }

    return resultTree;
  }

  private static TreeDto? BuildTree(List<TreeNode> items, int rootId)
  {
    if (items == null || items.Count == 0)
      return null;

    var lookup = new Dictionary<int, TreeDto>(items.Count);

    foreach (var item in items)
    {
      var dto = new TreeDto()
      {
        Id = item.Id,
        Name = item.Name,
        TreeName = item.TreeName,
      };
      lookup[item.Id] = dto;
    }

    foreach (var item in items)
    {
      if (item.ParentId.HasValue && lookup.TryGetValue(item.ParentId.Value, out var parent))
      {
        if (parent.Children == null)
        {
          parent.Children = [lookup[item.Id]];
        }
        else
        {
          parent.Children.Add(lookup[item.Id]);
        }
      }
    }

    return lookup[rootId];
  }
}
