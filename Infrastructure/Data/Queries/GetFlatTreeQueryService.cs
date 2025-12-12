using Core.TreeNodes;
using UseCases.Trees.Get;

namespace Infrastructure.Data.Queries;

public class GetFlatTreeQueryService(AppDbContext context) : IGetFlatTreeQueryService
{
  public async Task<List<TreeNode>> GetAsync(int Id)
  {
    var query = @"
      WITH RECURSIVE tree_cte AS (
          SELECT t.*, 0 as level
          FROM public.""TreeNodes"" t
          WHERE t.""Id""  = {0}
    
          UNION ALL
    
          SELECT t.*, tc.level + 1
          FROM public.""TreeNodes"" t
          INNER JOIN tree_cte tc ON t.""ParentId""  = tc.""Id"" 
      )
      SELECT * FROM tree_cte";

    return await context.TreeNodes
        .FromSqlRaw(query, Id)
        .AsNoTracking()
        .ToListAsync();
  }
}
