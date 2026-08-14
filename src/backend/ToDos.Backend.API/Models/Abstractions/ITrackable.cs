namespace ToDos.Backend.API.Models.Abstractions;

public interface ITrackable
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }

    void ChangeUpdatedAtTimestamp(DateTimeOffset? updatedAtTimestamp = null);
}
