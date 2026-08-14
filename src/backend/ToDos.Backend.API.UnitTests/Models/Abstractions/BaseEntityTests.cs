using ToDos.Backend.API.Models.Abstractions;

namespace ToDos.Backend.API.UnitTests.Models.Abstractions;

public class TestEntity : BaseEntity<int> { }

public class BaseEntityTests
{
    [Fact]
    public void ChangeUpdatedAtTimestamp_ShouldSetUpdatedAt_WhenTimestampIsNotProvided()
    {
        // Arrange
        var entity = new TestEntity();
        DateTimeOffset before = DateTimeOffset.UtcNow;

        // Act
        entity.ChangeUpdatedAtTimestamp();

        // Assert
        DateTimeOffset after = DateTimeOffset.UtcNow;
        Assert.NotNull(entity.UpdatedAt);
        Assert.True(entity.UpdatedAt >= before);
        Assert.True(entity.UpdatedAt <= after);
    }

    [Fact]
    public void ChangeUpdatedAtTimestamp_ShouldSetUpdatedAt_WhenTimestampIsProvided()
    {
        // Arrange
        var entity = new TestEntity();
        DateTimeOffset updateTimestamp = DateTimeOffset.UtcNow;

        // Act
        entity.ChangeUpdatedAtTimestamp(updateTimestamp);

        // Assert
        Assert.NotNull(entity.UpdatedAt);
        Assert.Equal(updateTimestamp, entity.UpdatedAt);
    }

    [Fact]
    public void SoftDelete_ShouldMarkEntityAsDeleted_WhenEntityIsNotDeleted()
    {
        // Arrange
        var entity = new TestEntity();
        DateTimeOffset before = DateTimeOffset.UtcNow;

        // Act
        entity.SoftDelete();

        // Assert
        DateTimeOffset after = DateTimeOffset.UtcNow;
        Assert.True(entity.IsDeleted);
        Assert.NotNull(entity.DeletedAt);
        Assert.True(entity.DeletedAt >= before);
        Assert.True(entity.DeletedAt <= after);
    }

    [Fact]
    public void SoftDelete_ShouldSetDeletedAtTimestamp_WhenTimestampIsProvided()
    {
        // Arrange
        var entity = new TestEntity();
        DateTimeOffset deleteTimestamp = DateTimeOffset.UtcNow;

        // Act
        entity.SoftDelete(deleteTimestamp);

        // Assert
        Assert.True(entity.IsDeleted);
        Assert.NotNull(entity.DeletedAt);
        Assert.Equal(deleteTimestamp, entity.DeletedAt);
    }

    [Fact]
    public void RestoreSoftDeleted_ShouldMarkEntityAsNotDeleted_WhenEntityIsSoftDeleted()
    {
        // Arrange
        var entity = new TestEntity();
        entity.SoftDelete();

        // Act
        entity.RestoreSoftDeleted();

        // Assert
        Assert.False(entity.IsDeleted);
        Assert.Null(entity.DeletedAt);
    }

    [Fact]
    public void RestoreSoftDeleted_ShouldNotChangeEntity_WhenEntityIsNotDeleted()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.RestoreSoftDeleted();

        // Assert
        Assert.False(entity.IsDeleted);
        Assert.Null(entity.DeletedAt);
    }
}
