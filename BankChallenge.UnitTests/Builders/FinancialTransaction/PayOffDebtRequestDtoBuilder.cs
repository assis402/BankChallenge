using BankChallenge.Shared.Dtos.FinancialTransaction;
using Bogus;

namespace BankChallenge.UnitTests.Builders.FinancialTransaction;

internal class PayOffDebtRequestDtoBuilder : BaseBuilder<PayOffDebtRequestDto>
{
    public override PayOffDebtRequestDtoBuilder Default()
    {
        var faker = new Faker();
        _instance = new PayOffDebtRequestDto(
            faker.Finance.Account(10),
            faker.Finance.Account(5),
            faker.Finance.Amount());

        return this;
    }

    public PayOffDebtRequestDtoBuilder Invalid()
    {
        _instance = new PayOffDebtRequestDto(null, "", 0);

        return this;
    }
}