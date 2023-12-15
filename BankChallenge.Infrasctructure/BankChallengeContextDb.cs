using BankChallenge.Business.Entities;
using BankChallenge.Shared;
using Matsoft.MongoDB;

namespace BankChallenge.Infrasctructure;

public class BankChallengeContextDb(bool isTestProject = false)
    : BaseContextDb(Settings.ConnectionString, Settings.Database, isTestProject)
{
    protected override void MapClasses()
    {
        RegisterMap<AccountHolderEntity>();
        RegisterMap<AccountEntity>();
        RegisterMap<FinancialTransactionEntity>();
        RegisterMap<DebtEntity>();
    }
}