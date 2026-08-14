namespace ToDos.Backend.API.Models.Abstractions;

public interface IArchivable
{
    bool IsArchived { get; set; }
    DateTimeOffset? ArchivedAt { get; set; }

    void Archive(DateTimeOffset? archivedAtTimestamp = null);
    void RestoreArchived();
}
