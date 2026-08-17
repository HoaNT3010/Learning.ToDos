using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDos.Backend.API.Utils;

namespace ToDos.Backend.API.UnitTests.Utils;

public class ProblemDetailsMapperTests
{
    [Fact]
    public void FromFluentValidation_Should_MapProblemDetailsMetadata()
    {
        // Arrange
        var exception = new ValidationException(
        [
            new ValidationFailure("Name", "Name is required.")
        ]);

        // Act
        ProblemDetails result = ProblemDetailsMapper.FromFluentValidation(exception);

        // Assert
        Assert.Equal("Validation error", result.Title);
        Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        Assert.Equal(
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            result.Type);
        Assert.Equal(
            "One or more validation errors occurred.",
            result.Detail);
    }

    [Fact]
    public void FromFluentValidation_Should_MapValidationErrors()
    {
        // Arrange
        var exception = new ValidationException(
        [
            new ValidationFailure("Name", "Name is required."),
            new ValidationFailure("Price", "Price must be greater than zero.")
        ]);

        // Act
        ProblemDetails result = ProblemDetailsMapper.FromFluentValidation(exception);

        // Assert
        Dictionary<string, string[]> errors = Assert.IsType<Dictionary<string, string[]>>(
            result.Extensions["errors"]);

        Assert.Equal(
            ["Name is required."],
            errors["name"]);

        Assert.Equal(
            ["Price must be greater than zero."],
            errors["price"]);
    }

    [Fact]
    public void FromFluentValidation_Should_GroupErrorsByProperty()
    {
        // Arrange
        var exception = new ValidationException(
        [
            new ValidationFailure("Name", "Name is required."),
            new ValidationFailure("Name", "Name must be at least 3 characters.")
        ]);

        // Act
        ProblemDetails result = ProblemDetailsMapper.FromFluentValidation(exception);

        // Assert
        Dictionary<string, string[]> errors = Assert.IsType<Dictionary<string, string[]>>(
            result.Extensions["errors"]);

        Assert.Equal(
            [
                "Name is required.",
                "Name must be at least 3 characters."
            ],
            errors["name"]);
    }

    [Fact]
    public void FromFluentValidation_Should_RemoveArrayIndexes()
    {
        // Arrange
        var exception = new ValidationException(
        [
            new ValidationFailure("Images[0]", "Image is required."),
            new ValidationFailure("Images[1]", "Image is invalid.")
        ]);

        // Act
        ProblemDetails result = ProblemDetailsMapper.FromFluentValidation(exception);

        // Assert
        Dictionary<string, string[]> errors = Assert.IsType<Dictionary<string, string[]>>(
            result.Extensions["errors"]);

        Assert.True(errors.ContainsKey("images"));
        Assert.Equal(
            [
                "Image is required.",
                "Image is invalid."
            ],
            errors["images"]);
    }

    [Fact]
    public void FromFluentValidation_Should_HandleNestedPropertyWithArrayIndex()
    {
        // Arrange
        var exception = new ValidationException(
        [
            new ValidationFailure(
                "Products[0].Price",
                "Price must be greater than zero.")
        ]);

        // Act
        ProblemDetails result = ProblemDetailsMapper.FromFluentValidation(exception);

        // Assert
        Dictionary<string, string[]> errors = Assert.IsType<Dictionary<string, string[]>>(
            result.Extensions["errors"]);

        Assert.True(errors.ContainsKey("products.Price"));
        Assert.Equal(
            ["Price must be greater than zero."],
            errors["products.Price"]);
    }
}
