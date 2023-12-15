using BankChallenge.Shared.Dtos.FinancialTransaction;
using Bogus;

namespace BankChallenge.UnitTests.Builders.FinancialTransaction;

internal class BaseTransactionRequestDtoBuilder : BaseBuilder<BaseTransactionRequestDto>
{
    public BaseTransactionRequestDtoBuilder()
    { }

    public BaseTransactionRequestDtoBuilder(string accountNumber, decimal amount)
    {
        _instance = new BaseTransactionRequestDto(accountNumber, amount);
    }

    public override BaseTransactionRequestDtoBuilder Default()
    {
        var faker = new Faker();
        _instance = new BaseTransactionRequestDto(faker.Finance.Account(5), faker.Finance.Amount(1, 20000));

        return this;
    }

    public BaseTransactionRequestDtoBuilder Invalid()
    {
        _instance = new BaseTransactionRequestDto(" ", 0);

        return this;
    }
}