namespace ToDos.Backend.API.Models.Abstractions;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTimeOffset? DeletedAt { get; set; }

    void SoftDelete(DateTimeOffset? deletedAtTimestamp = null);
    void RestoreSoftDeleted();
}
