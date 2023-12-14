namespace BankChallenge.Shared.Helpers;

public static class DateTimeUtils
{
    public static DateTime BrazilDateTime()
    {
        var timeUtc = DateTime.UtcNow;
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
    }
}