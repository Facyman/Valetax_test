using Core;

namespace Infrastructure.Data.Config;

public class JournalConfiguration : IEntityTypeConfiguration<Journal>
{
  public void Configure(EntityTypeBuilder<Journal> builder)
  {
    builder.HasKey(e => e.Id);

    builder.HasIndex(e => e.EventId).IsUnique();

    builder.Property(e => e.CreatedAt)
      .HasDefaultValueSql("NOW()"); ;
  }
}
