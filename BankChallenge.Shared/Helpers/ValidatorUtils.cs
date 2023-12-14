using FluentValidation;

namespace BankChallenge.Shared.Helpers;

public static class ValidatorUtils
{
    public static IRuleBuilderOptions<T, string> ValidCpf<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder.Must(cpf => cpf.IsValidCpf());

    public static IRuleBuilderOptions<T, string> ValidEnum<T, TEnum>(this IRuleBuilder<T, string> ruleBuilder)
        where TEnum : Enum =>
        ruleBuilder.Must(stringEnum =>
            Enum.GetNames(typeof(TEnum))
                .Any(x => string.Equals(x.ToLower(), stringEnum.ToLower(), StringComparison.Ordinal))
        );

    private static bool IsValidCpf(this string cpf)
    {
        cpf = cpf.RemoveNonNumeric();

        if (cpf.Length != 11) return false;

        var repeated = true;

        for (var i = 1; i < cpf.Length; i++)
        {
            if (cpf[i] != cpf[0])
            {
                repeated = false;
                break;
            }
        }

        if (repeated || cpf == "12345678909") return false;

        var cpfArray = cpf.Select(x => int.Parse(x.ToString())).ToArray();

        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += cpfArray[i] * (10 - i);

        var remnant = (sum * 10) % 11;

        if (remnant == 10 && cpfArray[9] != 0) return false;
        if (remnant != 10 && remnant != cpfArray[9]) return false;

        sum = 0;
        for (var i = 0; i < 10; i++) sum += cpfArray[i] * (11 - i);

        remnant = (sum * 10) % 11;

        if (remnant == 10 && cpfArray[10] != 0) return false;
        if (remnant != 10 && remnant != cpfArray[10]) return false;

        return true;
    }
}