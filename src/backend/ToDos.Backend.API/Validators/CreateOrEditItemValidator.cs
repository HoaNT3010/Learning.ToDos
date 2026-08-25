using FluentValidation;
using ToDos.Backend.API.DTOs;
using ToDos.Backend.API.Models.Enums;

namespace ToDos.Backend.API.Validators;

public class CreateOrEditItemValidator : AbstractValidator<CreateOrEditItemRequest>
{
    public CreateOrEditItemValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Item's name is required.")
            .MaximumLength(100)
            .WithMessage("Item's name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Item's description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Priority)
            .IsEnumName(typeof(ToDoItemPriority))
            .WithMessage("Item's priority must be valid.");
    }
}
