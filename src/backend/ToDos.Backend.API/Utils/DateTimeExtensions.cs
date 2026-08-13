namespace ToDos.Backend.API.Utils;

public static class DateTimeExtensions
{
    public static DateTime StartOfDay(this DateTime dateTime) => dateTime.Date;
    public static DateTime EndOfDay(this DateTime dateTime) => dateTime.Date.AddDays(1).AddTicks(-1);
}
