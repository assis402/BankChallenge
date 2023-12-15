using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using Bogus;

namespace BankChallenge.UnitTests.Builders.Account;

internal class AccountEntityBuilder : BaseBuilder<AccountEntity>
{
    public AccountEntityBuilder Default(string accountHolderId)
    {
        var faker = new Faker();

        _instance = new AccountEntity
        {
            AccountAgency = faker.Finance.Account(4),
            AccountNumber = faker.Finance.Account(5),
            AccountHolderId = accountHolderId,
            Balance = faker.Finance.Amount(20000, 200000),
            Type = faker.PickRandom<AccountType>(),
            Status = AccountStatus.Active,
            CreatedDate = faker.Date.Past()
        };

        return this;
    }
}