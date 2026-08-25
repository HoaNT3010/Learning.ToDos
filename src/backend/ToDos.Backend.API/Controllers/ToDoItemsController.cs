using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using ToDos.Backend.API.DTOs;
using ToDos.Backend.API.Services;
using ToDos.Backend.API.Utils;

namespace ToDos.Backend.API.Controllers;

[ApiController]
[Route("api/items")]
public class ToDoItemsController : ControllerBase
{
    readonly IToDoItemService _itemService;

    public ToDoItemsController(IToDoItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems([FromQuery] GetToDoItemsRequest request, CancellationToken cancellationToken)
    {
        ErrorOr<PagedResponse<ToDoItemResponse>> result = await _itemService.GetItems(request, cancellationToken);
        return result.Match(
            Ok,
            errors => errors.ToProblem()
        );
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] CreateOrEditItemRequest request, CancellationToken cancellationToken)
    {
        ErrorOr<ToDoItemResponse> result = await _itemService.CreateItem(request, cancellationToken);
        return result.Match(
            Ok,
            errors => errors.ToProblem()
        );
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateItem(string id, CreateOrEditItemRequest request, CancellationToken cancellationToken)
    {
        ErrorOr<ToDoItemResponse> result = await _itemService.EditItem(id, request, cancellationToken);
        return result.Match(
            Ok,
            errors => errors.ToProblem()
        );
    }

    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> MarkAsCompleted(string id, CancellationToken cancellationToken)
    {
        ErrorOr<bool> result = await _itemService.MarkAsCompleted(id, cancellationToken);
        return result.Match(
            value => NoContent(),
            errors => errors.ToProblem()
        );
    }
}
