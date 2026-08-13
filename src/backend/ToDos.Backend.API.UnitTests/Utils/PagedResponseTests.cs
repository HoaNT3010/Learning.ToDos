using ToDos.Backend.API.Utils;

namespace ToDos.Backend.API.UnitTests.Utils;

public class PagedResponseTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeDefaultValues()
    {
        // Arrange
        var response = new PagedResponse<string>();

        // Assert
        Assert.Empty(response.Items);
        Assert.Equal(0, response.PageSize);
        Assert.Equal(0, response.PageNumber);
        Assert.Equal(0, response.TotalPages);
        Assert.Equal(0, response.TotalCount);
        Assert.False(response.HasPreviousPage);
        Assert.False(response.HasNextPage);
    }

    [Fact]
    public void Constructor_ShouldSetPaginationProperties()
    {
        // Arrange
        var items = new List<string> { "Item 1", "Item 2", "Item 3" };

        // Act
        var response = new PagedResponse<string>(
            items,
            totalCount: 10,
            pageNumber: 1,
            pageSize: 3);

        // Assert
        Assert.Equal(items, response.Items);
        Assert.Equal(1, response.PageNumber);
        Assert.Equal(3, response.PageSize);
        Assert.Equal(10, response.TotalCount);
        Assert.Equal(4, response.TotalPages);
    }

    [Fact]
    public void HasPreviousPage_ShouldBeFalse_WhenOnFirstPage()
    {
        // Arrange
        var response = new PagedResponse<string>(
            [],
            totalCount: 10,
            pageNumber: 1,
            pageSize: 3);

        // Assert
        Assert.False(response.HasPreviousPage);
    }

    [Fact]
    public void HasPreviousPage_ShouldBeTrue_WhenNotOnFirstPage()
    {
        // Arrange
        var response = new PagedResponse<string>(
            [],
            totalCount: 10,
            pageNumber: 2,
            pageSize: 3);

        // Assert
        Assert.True(response.HasPreviousPage);
    }

    [Fact]
    public void HasNextPage_ShouldBeTrue_WhenNotOnLastPage()
    {
        // Arrange
        var response = new PagedResponse<string>(
            [],
            totalCount: 10,
            pageNumber: 2,
            pageSize: 3);

        // Assert
        Assert.True(response.HasNextPage);
    }

    [Fact]
    public void HasNextPage_ShouldBeFalse_WhenOnLastPage()
    {
        // Arrange
        var response = new PagedResponse<string>(
            [],
            totalCount: 10,
            pageNumber: 4,
            pageSize: 3);

        // Assert
        Assert.False(response.HasNextPage);
    }

    [Theory]
    [InlineData(10, 3, 4)]
    [InlineData(10, 5, 2)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 5, 3)]
    [InlineData(0, 5, 0)]
    public void Constructor_ShouldCalculateTotalPagesCorrectly(
        int totalCount,
        int pageSize,
        int expectedTotalPages)
    {
        // Arrange
        var response = new PagedResponse<string>(
            [],
            totalCount,
            pageNumber: 1,
            pageSize);

        // Assert
        Assert.Equal(expectedTotalPages, response.TotalPages);
    }
}
