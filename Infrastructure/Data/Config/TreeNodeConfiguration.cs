using Core.TreeNodes;

namespace Infrastructure.Data.Config;

public class TreeNodeConfiguration : IEntityTypeConfiguration<TreeNode>
{
  public void Configure(EntityTypeBuilder<TreeNode> builder)
  {
    builder.HasKey(e => e.Id);

    builder.HasIndex(e => e.Name)
        .HasDatabaseName("IX_tree_nodes_name")
        .IsUnique(false);

    builder.HasIndex(e => e.ParentId)
        .HasDatabaseName("IX_tree_nodes_parent_id")
        .IsUnique(false);

    builder.HasIndex(e => new { e.ParentId, e.Id })
        .HasDatabaseName("IX_tree_nodes_parent_id_id");

    builder.Property(entity => entity.Id)
      .IsRequired();

    builder.Property(entity => entity.Name)
      .IsRequired();

    builder.Property(entity => entity.TreeName)
      .IsRequired();

    builder.HasOne(e => e.Parent)
      .WithMany(e => e.Children)
      .HasForeignKey(e => e.ParentId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
