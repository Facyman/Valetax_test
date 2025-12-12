using Ardalis.SharedKernel;
using Core.TreeNodes;
using Core.TreeNodes.Specifications;
using NSubstitute;
using UseCases.Trees;
using UseCases.Trees.Get;

namespace UnitTests.UseCases.Trees;

public class GetTreeHandlerHandle
{
  private readonly string _testName1 = "test tree node1";
  private readonly string _testName2 = "test tree node2";
  private readonly string _testName3 = "test tree node3";
  private readonly string _testName4 = "test tree node4";
  private readonly string _testTreeName = "test tree";
  private readonly IReadRepository<TreeNode> _repository = Substitute.For<IReadRepository<TreeNode>>();
  private readonly IGetFlatTreeQueryService _flatTreeQuery = Substitute.For<IGetFlatTreeQueryService>();
  private readonly GetTreeHandler _handler;

  public GetTreeHandlerHandle()
  {
    _handler = new GetTreeHandler(_repository, _flatTreeQuery);
  }

  private TreeNode? CreateTreeNode()
  {
    return new TreeNode(_testName1, _testTreeName)
    {
      Id = 1,
    };
  }

  private List<TreeNode> CreateFlatTree()
  {
    return
    [
      new TreeNode(_testName1, _testTreeName)
    {
      Id = 1,
    },
      new TreeNode(_testName4, _testTreeName, 1)
    {
      Id = 4,
    },
      new TreeNode(_testName3, _testTreeName, 1)
    {
      Id = 3,
    },
      new TreeNode(_testName2, _testTreeName, 4)
    {
      Id = 2,
    }
    ];
  }

  private TreeDto? CreateTree()
  {
    return new TreeDto(){
      Id = 1, 
      Name = _testName1, 
      TreeName = _testTreeName,
      Children = [
        new TreeDto() { Id = 4, Name = _testName4, TreeName= _testTreeName,
        Children = [ new TreeDto() { Id = 2, Name = _testName2, TreeName = _testTreeName }]},
        new TreeDto() { Id = 3, Name = _testName3, TreeName = _testTreeName }
      ]};
  }

  [Fact]
  public async Task ReturnsValidTree()
  {
    //Arrange
    _repository.FirstOrDefaultAsync(Arg.Any<TreeNodeByParentIdAndNameSpec>(), Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(CreateTreeNode()));
    _flatTreeQuery.GetAsync(Arg.Any<int>()).Returns(Task.FromResult(CreateFlatTree()));

    //Act
    var result = await _handler.Handle(new GetTreeQuery(_testName1), CancellationToken.None);

    //Assert
    var expectedTree = CreateTree();
    Assert.Equal(expectedTree, result);
  }
}
