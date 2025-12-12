using System;
using System.Text.Json.Serialization;

namespace UseCases.Trees;

public record TreeDto
{
  public required int Id { get; set; }
  public required string Name { get; set; }
  public required string TreeName { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public List<TreeDto>? Children { get; set; }

  public virtual bool Equals(TreeDto? other)
  {
    if (ReferenceEquals(this, other)) return true;
    if (other is null) return false;

    return Id == other.Id &&
           Name == other.Name &&
           TreeName == other.TreeName &&
           (Children is null ? other.Children is null :
            other.Children is not null && Children.SequenceEqual(other.Children));
  }

  public override int GetHashCode()
  {
    var hash = new HashCode();
    hash.Add(Id);
    hash.Add(Name);
    hash.Add(TreeName);

    if (Children is not null)
    {
      foreach (var child in Children)
      {
        hash.Add(child.GetHashCode());
      }
    }

    return hash.ToHashCode();
  }
};
