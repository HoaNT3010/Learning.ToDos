using ToDos.Backend.API.Models;
using ToDos.Backend.API.Models.Enums;

namespace ToDos.Backend.API.UnitTests.Models;

public class ToDoItemTests
{
    [Fact]
    public void Create_ShouldReturnDefaultItem_WhenProvideDefaultValues()
    {
        // Arrange
        var itemName = "Write unit tests";
        DateTimeOffset before = DateTimeOffset.UtcNow;

        // Act
        var item = ToDoItem.Create(itemName);

        // Assert
        DateTimeOffset after = DateTimeOffset.UtcNow;
        Assert.NotNull(item);
        Assert.Equal(itemName, item.Name);
        Assert.Null(item.Description);
        Assert.Equal(ToDoItemPriority.Medium, item.Priority);
        Assert.Null(item.UpdatedAt);
        Assert.False(item.IsDeleted);
        Assert.Null(item.DeletedAt);
        Assert.False(item.IsArchived);
        Assert.Null(item.ArchivedAt);
        Assert.True(item.CreatedAt >= before);
        Assert.True(item.CreatedAt <= after);
    }

    [Fact]
    public void Create_ShouldReturnItemWithValues_WhenParametersAreSpecified()
    {
        // Arrange
        var itemName = "Write unit tests";
        var itemDescription = "Write unit tests for the ToDoItem model.";
        DateTimeOffset createTimestamp = DateTimeOffset.UtcNow;
        ToDoItemPriority itemPriority = ToDoItemPriority.Important;

        // Act
        var item = ToDoItem.Create(itemName, itemDescription, itemPriority, createTimestamp);

        // Assert
        Assert.NotNull(item);
        Assert.Equal(itemName, item.Name);
        Assert.Equal(itemDescription, item.Description);
        Assert.Equal(itemPriority, item.Priority);
        Assert.Equal(createTimestamp, item.CreatedAt);
    }

    [Fact]
    public void Archive_ShouldMarkItemAsArchived_WhenItemIsNotArchived()
    {
        // Arrange
        DateTimeOffset archiveTimestamp = DateTimeOffset.UtcNow;
        var item = ToDoItem.Create("Item 1");

        // Act
        item.Archive(archiveTimestamp);

        // Assert
        Assert.True(item.IsArchived);
        Assert.Equal(archiveTimestamp, item.ArchivedAt);
    }

    [Fact]
    public void RestoreArchived_ShouldMarkItemAsNotArchived_WhenItemIsArchived()
    {
        // Arrange
        var item = ToDoItem.Create("Item 2");
        item.Archive();

        // Act
        item.RestoreArchived();

        // Assert
        Assert.False(item.IsArchived);
        Assert.Null(item.ArchivedAt);
    }

    [Fact]
    public void RestoreArchived_ShouldNotChangeItem_WhenItemIsNotArchived()
    {
        // Arrange
        var item = ToDoItem.Create("Item 3");

        // Act
        item.RestoreArchived();

        // Assert
        Assert.False(item.IsArchived);
        Assert.Null(item.ArchivedAt);
    }
}
