using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using Bogus;

namespace BankChallenge.UnitTests.Builders.Debt;

internal class DebtEntityBuilder : BaseBuilder<DebtEntity>
{
    public override DebtEntityBuilder Default()
    {
        var faker = new Faker();

        _instance = new DebtEntity
        {
            RequestedLoanAmount = faker.Finance.Amount(),
            OriginalAmountToPay = faker.Finance.Amount(),
            CurrentAmountToPay = faker.Finance.Amount(),
            AccountId = faker.Finance.Account(10),
            DueDate = faker.Date.FutureDateOnly(),
            Type = faker.PickRandom<DebtType>(),
            Status = DebtStatus.Pending,
            CreatedDate = faker.Date.Past()
        };

        return this;
    }

    public DebtEntityBuilder WithAccountId(string accountId)
    {
        _instance.AccountId = accountId;
        return this;
    }

    public DebtEntityBuilder WithCurrentAmountToPay(decimal amount)
    {
        _instance.CurrentAmountToPay = amount;
        return this;
    }
}