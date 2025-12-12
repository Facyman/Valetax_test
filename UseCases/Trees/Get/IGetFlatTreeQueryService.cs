using Core.TreeNodes;

namespace UseCases.Trees.Get
{
  public interface IGetFlatTreeQueryService
  {
    Task<List<TreeNode>> GetAsync(int Id);
  }
}
