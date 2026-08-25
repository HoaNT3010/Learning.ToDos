using FluentValidation.TestHelper;
using ToDos.Backend.API.DTOs;
using ToDos.Backend.API.Validators;

namespace ToDos.Backend.API.UnitTests.Validators;

public class GetToDoItemsValidatorTests
{
    readonly GetToDoItemsValidator _validatior = new();

    [Fact]
    public void Validate_ShouldNotHaveValidationError_WhenRequestIsValid()
    {
        // Arrange
        var request = new GetToDoItemsRequest();

        // Act
        TestValidationResult<GetToDoItemsRequest> result = _validatior.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
        Assert.Equal(1, request.PageNumber);
        Assert.Equal(20, request.PageSize);
    }

    [Theory]
    [InlineData("createAt")]
    [InlineData("id")]
    [InlineData("property123")]
    public void Validate_ShouldHaveValidationError_WhenSortByIsNotValid(string sortBy)
    {
        // Arrange
        var request = new GetToDoItemsRequest() { SortBy = sortBy };

        // Act
        TestValidationResult<GetToDoItemsRequest> result = _validatior.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SortBy);
    }
}
