using ErrorOr;
using ToDos.Backend.API.DTOs;
using ToDos.Backend.API.Utils;

namespace ToDos.Backend.API.Services;

public interface IToDoItemService
{
    Task<ErrorOr<PagedResponse<ToDoItemResponse>>> GetItems(GetToDoItemsRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<ToDoItemResponse>> CreateItem(CreateOrEditItemRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<ToDoItemResponse>> EditItem(string? itemId, CreateOrEditItemRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<bool>> MarkAsCompleted(string? itemId, CancellationToken cancellationToken);
}
