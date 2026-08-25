using ToDos.Backend.API.Models.Abstractions;
using ToDos.Backend.API.Models.Enums;

namespace ToDos.Backend.API.Models;

public sealed class ToDoItem : BaseEntity<Guid>, IArchivable
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ToDoItemPriority Priority { get; set; }
    public bool IsArchived { get; set; }
    public DateTimeOffset? ArchivedAt { get; set; }

    public void Archive(DateTimeOffset? archivedAtTimestamp = null)
    {
        if (IsArchived) return;
        IsArchived = true;
        ArchivedAt = archivedAtTimestamp ?? DateTimeOffset.UtcNow;
    }
    public void RestoreArchived()
    {
        if (!IsArchived) return;
        IsArchived = false;
        ArchivedAt = null;
    }

    public static ToDoItem Create(
        string name,
        string? description = null,
        ToDoItemPriority priority = ToDoItemPriority.Medium,
        DateTimeOffset? createTimestamp = null)
    {
        return new ToDoItem
        {
            Name = name,
            Description = description,
            Priority = priority,
            CreatedAt = createTimestamp ?? DateTimeOffset.UtcNow,
        };
    }

    public bool CanMarkAsCompleted() => !IsArchived && !IsDeleted;
}
