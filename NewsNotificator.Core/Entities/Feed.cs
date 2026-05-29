using System.ComponentModel.DataAnnotations;

namespace NewsNotificator.Core.Entities;

public class Feed
{
  [Key]
  public int Id { get; init; }
  public required Uri Uri { get; init; }
  [MaxLength(100)]
  public required string Title { get; init; }
}
