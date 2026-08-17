namespace ToDos.Backend.API.DTOs;

public record CreateOrEditItemRequest(
    string Name,
    string? Description,
    string Priority
);
