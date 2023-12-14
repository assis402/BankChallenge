using System.Text;
using System.Text.RegularExpressions;

namespace BankChallenge.Shared.Helpers;

public static partial class StringUtils
{
    public static string FirstCharToLowerCase(this string @string)
        => char.ToLowerInvariant(@string[0]) + @string[1..];

    public static byte[] ConvertToAscii(this string text)
        => Encoding.ASCII.GetBytes(text);

    public static string RemoveNonNumeric(this string @string)
        => NonNumericRegex().Replace(@string, string.Empty);

    [GeneratedRegex("[^0-9]")]
    private static partial Regex NonNumericRegex();
}