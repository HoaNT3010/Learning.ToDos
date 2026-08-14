namespace ToDos.Backend.API.Models.Abstractions;

public abstract class BaseEntity<TKey> : IHasKey<TKey>, ITrackable, ISoftDeletable
{
    public TKey Id { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual void ChangeUpdatedAtTimestamp(DateTimeOffset? updatedAtTimestamp = null) => UpdatedAt = updatedAtTimestamp ?? DateTimeOffset.UtcNow;

    public virtual void SoftDelete(DateTimeOffset? deletedAtTimestamp = null)
    {
        if (IsDeleted) return;
        IsDeleted = true;
        DeletedAt = deletedAtTimestamp ?? DateTimeOffset.UtcNow;
    }

    public virtual void RestoreSoftDeleted()
    {
        if (!IsDeleted) return;
        IsDeleted = false;
        DeletedAt = null;
    }
}
