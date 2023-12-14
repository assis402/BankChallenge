using BankChallenge.Business.Entities;
using BankChallenge.Shared;
using Matsoft.MongoDB;

namespace BankChallenge.Infrasctructure;

public class BankChallengeContextDb() : BaseContextDb(Settings.ConnectionString, Settings.Database)
{
    protected override void MapClasses()
    {
        RegisterMap<AccountHolderEntity>();
        RegisterMap<AccountEntity>();
        RegisterMap<FinancialTransactionEntity>();
        RegisterMap<DebtEntity>();
    }
}