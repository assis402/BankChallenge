namespace BankChallenge.Shared.Helpers;

public static class NumberUtils
{
    public static int RandomNumber(int min, int max)
    {
        var random = new Random();

        lock (random)
        {
            return random.Next(min, max);
        }
    }
}