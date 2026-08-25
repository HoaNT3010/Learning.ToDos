using ToDos.Backend.API.DTOs;
using ToDos.Backend.API.Validators;
using FluentValidation.TestHelper;

namespace ToDos.Backend.API.UnitTests.Validators;

public class CreateOrEditItemValidatorTests
{
    readonly CreateOrEditItemValidator _validator = new();

    [Fact]
    public void Validate_ShouldNotHaveValidationError_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateOrEditItemRequest("Item 1", "Item description.", "Medium");

        // Act
        TestValidationResult<CreateOrEditItemRequest> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Normal")]
    [InlineData("Hello world!")]
    public void Validate_ShouldHaveValidationError_WhenProrityIsInvalid(string prioriy)
    {
        // Arrange
        var request = new CreateOrEditItemRequest("Item", null, prioriy);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Priority)
            .WithErrorMessage("Item's priority must be valid.");
    }
}
