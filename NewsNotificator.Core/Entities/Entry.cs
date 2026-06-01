using System.ComponentModel.DataAnnotations;

namespace NewsNotificator.Core.Entities;

public class Entry
{
  [Key, MaxLength(256)]
  public required string Guid { get; init; }
  [MaxLength(256)]
  public required string Title { get; init; }
  // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
  public string? Description { get; init; }
  public required Uri Link { get; init; }
  public required DateTime PubDate { get; init; }

  public required Feed Feed { get; init; }
}
