using ToDos.Backend.API.Utils;

namespace ToDos.Backend.API.UnitTests.Utils;

public class DateTimeExtensionsTests
{
    [Fact]
    public void StartOfDay_ShouldReturnStartOfDay()
    {
        var dateTime = new DateTime(2002, 10, 30, 12, 12, 12, 500, DateTimeKind.Local);

        var result = dateTime.StartOfDay();

        Assert.Equal(new DateTime(2002, 10, 30, 0, 0, 0, DateTimeKind.Local), result);
        Assert.Equal(TimeSpan.Zero, result.TimeOfDay);
    }

    [Theory]
    [InlineData(10, 30, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(14, 27, 46)]
    [InlineData(23, 59, 59)]
    public void StartOfDay_ShouldAlwaysReturnMidnight(int hour, int minute, int second)
    {
        // Arrange
        var dateTime = new DateTime(2026, 10, 8, hour, minute, second);

        // Act
        var result = dateTime.StartOfDay();

        // Assert
        Assert.Equal(new DateTime(2026, 10, 8, 0, 0, 0), result);
        Assert.Equal(TimeSpan.Zero, result.TimeOfDay);
    }

    [Fact]
    public void EndOfDate_ShouldReturnEndOfDay()
    {
        // Arrange
        var dateTime = new DateTime(2002, 10, 30, 12, 12, 12, 500, DateTimeKind.Local);

        // Act
        var result = dateTime.EndOfDay();

        // Assert
        Assert.Equal(new DateTime(2002, 10, 30, 23, 59, 59, 999, DateTimeKind.Local).AddTicks(9999), result);
        Assert.Equal(new DateTime(2002, 10, 30, 0, 0, 0, DateTimeKind.Local).AddDays(1).AddTicks(-1), result);
    }

    [Theory]
    [InlineData(10, 30, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(14, 27, 46)]
    [InlineData(23, 59, 59)]
    public void EndOfDay_ShouldAlwaysReturnLastTickOfDay(int hour, int minute, int second)
    {
        // Arrange
        var dateTime = new DateTime(2026, 10, 8, hour, minute, second);

        // Act
        var result = dateTime.EndOfDay();

        // Assert
        Assert.Equal(new DateTime(2026, 10, 8, 23, 59, 59, 999, DateTimeKind.Local).AddTicks(9999), result);
        Assert.Equal(new DateTime(2026, 10, 8, 0, 0, 0, DateTimeKind.Local).AddDays(1).AddTicks(-1), result);
    }
}
