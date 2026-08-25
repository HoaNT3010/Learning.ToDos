using FluentValidation;
using ToDos.Backend.API.DTOs;

namespace ToDos.Backend.API.Validators;

public class GetToDoItemsValidator : AbstractValidator<GetToDoItemsRequest>
{
    static readonly string[] _sortKeys = ["name", "priority"];

    public GetToDoItemsValidator()
    {
        RuleFor(x => x.Keyword)
            .MaximumLength(100)
            .WithMessage("Keyword cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Keyword));

        RuleFor(x => x.SortBy)
            .Must(value => _sortKeys.Contains(value, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Sort by must be on of: {string.Join(", ", _sortKeys)}")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy));

        RuleFor(x => x.SortDirection)
            .IsInEnum()
            .WithMessage("Sort direction must be a valid value.")
            .When(x => x.SortDirection.HasValue);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.")
            .When(x => x.PageNumber.HasValue);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be less than or equal to 100.")
            .When(x => x.PageSize.HasValue);
    }
}
