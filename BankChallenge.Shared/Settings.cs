namespace BankChallenge.Shared;

public static class Settings
{
    public static readonly string AccessTokenSecret = Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET");
    public static readonly string ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    public static readonly string Database = Environment.GetEnvironmentVariable("DATABASE");
}