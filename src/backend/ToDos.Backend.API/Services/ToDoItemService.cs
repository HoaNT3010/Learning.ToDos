using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using ToDos.Backend.API.Data;
using ToDos.Backend.API.DTOs;
using ToDos.Backend.API.DTOs.Enums;
using ToDos.Backend.API.Errors;
using ToDos.Backend.API.Models;
using ToDos.Backend.API.Models.Enums;
using ToDos.Backend.API.Utils;
using ToDos.Backend.API.Validators;

namespace ToDos.Backend.API.Services;

public class ToDoItemService : IToDoItemService
{
    readonly AppDbContext _context;
    readonly DbSet<ToDoItem> _itemSet;
    readonly CreateOrEditItemValidator _createOrEditValidator;
    readonly GetToDoItemsValidator _getItemsValidator;

    public ToDoItemService(
        AppDbContext context,
        CreateOrEditItemValidator createOrEditValidator,
        GetToDoItemsValidator getItemsValidator)
    {
        _context = context;
        _itemSet = _context.Set<ToDoItem>();
        _createOrEditValidator = createOrEditValidator;
        _getItemsValidator = getItemsValidator;
    }

    public async Task<ErrorOr<ToDoItemResponse>> CreateItem(
        CreateOrEditItemRequest request,
        CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _createOrEditValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        CreateOrEditItemRequest normalizedRequest = request with
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
        };

        var newItem = ToDoItem.Create(
            normalizedRequest.Name,
            normalizedRequest.Description,
            Enum.Parse<ToDoItemPriority>(normalizedRequest.Priority));

        await _itemSet.AddAsync(newItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new ToDoItemResponse(newItem.Id,
            newItem.Name,
            newItem.Description,
            newItem.Priority.ToString(),
            newItem.CreatedAt,
            newItem.IsArchived);
    }
    public async Task<ErrorOr<ToDoItemResponse>> EditItem(string? itemId, CreateOrEditItemRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _createOrEditValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        CreateOrEditItemRequest normalizedRequest = request with
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
        };

        Guid id = ValidateItemId(itemId);
        ToDoItem? item = await _itemSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (item is null) return ToDoItemErrors.ItemNotFound;

        item.Name = normalizedRequest.Name;
        item.Description = normalizedRequest.Description;
        item.Priority = Enum.Parse<ToDoItemPriority>(normalizedRequest.Priority);
        await _context.SaveChangesAsync(cancellationToken);
        return new ToDoItemResponse(item.Id,
            item.Name,
            item.Description,
            item.Priority.ToString(),
            item.CreatedAt,
            item.IsArchived);
    }

    public async Task<ErrorOr<PagedResponse<ToDoItemResponse>>> GetItems(GetToDoItemsRequest request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _getItemsValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        IQueryable<ToDoItem> filterQuery = ApplyItemsFilter(request, _itemSet);
        var totalCount = await filterQuery.CountAsync(cancellationToken);
        IQueryable<ToDoItem> pagingQuery = ApplyItemsSortingAndPaging(request, filterQuery);
        var projQuery = pagingQuery.Select(x => new ToDoItemResponse(
            x.Id,
            x.Name,
            x.Description,
            x.Priority.ToString(),
            x.CreatedAt,
            x.IsArchived));
        List<ToDoItemResponse> items = await projQuery.ToListAsync(cancellationToken);

        return new PagedResponse<ToDoItemResponse>(items, totalCount, request.PageNumber ?? 1, request.PageSize ?? 20);

    }

    public async Task<ErrorOr<bool>> MarkAsCompleted(string? itemId, CancellationToken cancellationToken)
    {
        Guid id = ValidateItemId(itemId);
        ToDoItem? item = await _itemSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (item is null) return ToDoItemErrors.ItemNotFound;

        if (!item.CanMarkAsCompleted()) return ToDoItemErrors.CannotMarkAsCompleted;
        item.Archive();
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static Guid ValidateItemId(string? itemId)
    {
        var result = Guid.TryParse(itemId, out Guid id);
        if (!result) throw new ValidationException([new ValidationFailure(itemId, "Item's identifier must be in GUID/UUID format.")]);
        return id;
    }

    private static IQueryable<ToDoItem> ApplyItemsFilter(GetToDoItemsRequest request, IQueryable<ToDoItem> query)
    {
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            query = query.Where(x => x.Name.Contains(request.Keyword.Trim()));
        }
        if (request.IsArchived)
        {
            query = query.Where(x => x.IsArchived);
        }
        query = query.Where(x => !x.IsDeleted);

        return query;
    }

    private static IQueryable<ToDoItem> ApplyItemsSortingAndPaging(GetToDoItemsRequest request, IQueryable<ToDoItem> query)
    {
        var sortBy = request.SortBy?.ToLowerInvariant();
        query = sortBy switch
        {
            "name" => request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(x => x.Name).ThenByDescending(x => x.CreatedAt)
                        : query.OrderByDescending(x => x.Name).ThenByDescending(x => x.CreatedAt),
            "priority" => request.SortDirection == SortDirection.Ascending
                            ? query.OrderBy(
                                x => x.Priority == ToDoItemPriority.None ? 0 :
                                x.Priority == ToDoItemPriority.Low ? 1 :
                                x.Priority == ToDoItemPriority.Medium ? 2 :
                                x.Priority == ToDoItemPriority.High ? 3 : 4)
                            .ThenByDescending(x => x.CreatedAt)
                            : query.OrderByDescending(
                                x => x.Priority == ToDoItemPriority.None ? 0 :
                                x.Priority == ToDoItemPriority.Low ? 1 :
                                x.Priority == ToDoItemPriority.Medium ? 2 :
                                x.Priority == ToDoItemPriority.High ? 3 : 4)
                            .ThenByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt),
        };
        if (request.PageNumber.HasValue && request.PageSize.HasValue)
        {
            query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                        .Take(request.PageSize.Value);
        }

        return query;
    }
}
