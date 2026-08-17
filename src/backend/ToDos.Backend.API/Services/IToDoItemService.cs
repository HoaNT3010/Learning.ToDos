using ErrorOr;
using ToDos.Backend.API.DTOs;
using ToDos.Backend.API.Utils;

namespace ToDos.Backend.API.Services;

public interface IToDoItemService
{
    Task<ErrorOr<PagedResponse<ToDoItemResponse>>> GetItems(GetToDoItemsRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<ToDoItemResponse>> CreateItem(CreateOrEditItemRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<ToDoItemResponse>> EditItem(Guid itemId, CreateOrEditItemRequest request, CancellationToken cancellationToken);
    Task<ErrorOr<bool>> MarkAsCompleted(Guid itemId, CancellationToken cancellationToken);
}
