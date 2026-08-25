namespace ToDos.Backend.API.DTOs;

public record ToDoItemResponse(
    Guid Id,
    string Name,
    string? Description,
    string Priority,
    DateTimeOffset CreatedAt,
    bool IsArchived
);
