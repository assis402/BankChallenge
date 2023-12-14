using BankChallenge.Business.Enums;
using BankChallenge.Shared.Dtos.Account;
using BankChallenge.Shared.Helpers;
using Matsoft.MongoDB;
using Matsoft.MongoDB.CustomAttributes;

namespace BankChallenge.Business.Entities;

[CollectionName("account")]
public class AccountEntity : BaseEntity
{
    public AccountEntity(string accountHolderId)
    {
        AccountHolderId = accountHolderId;
        Balance = 0;
        Type = AccountType.CheckingAccount;
        Status = AccountStatus.Active;
    }

    public static implicit operator AccountDto(AccountEntity entity) =>
        new(entity.AccountAgency,
            entity.AccountNumber,
            entity.Balance,
            entity.Type.ToString(),
            entity.Status.ToString(),
            DateOnly.FromDateTime(entity.CreatedDate));

    public string AccountAgency = "0001"; // Por ser um banco digital, possui apenas uma agência

    public string AccountNumber { get; private set; }

    public string AccountHolderId { get; private set; }

    public decimal Balance { get; private set; }

    public AccountType Type { get; private set; }

    public AccountStatus Status { get; private set; }

    public bool IsActive() => Status == AccountStatus.Active;

    public bool HasSufficientBalance(decimal amountToPay) => Balance >= amountToPay;

    public void ExecuteFinancialTransaction(FinancialTransactionEntity financialTransactionEntity)
    {
        switch (financialTransactionEntity.Category)
        {
            case FinancialTransactionCategory.Income:
                Balance += financialTransactionEntity.Amount;
                break;

            case FinancialTransactionCategory.Outcome:
                Balance -= financialTransactionEntity.Amount;
                break;
        }

        financialTransactionEntity.SetInProcess();
    }

    public void ReverseFinancialTransaction(FinancialTransactionEntity financialTransactionEntity)
    {
        switch (financialTransactionEntity.Category)
        {
            case FinancialTransactionCategory.Income:
                Balance -= financialTransactionEntity.Amount;
                break;

            case FinancialTransactionCategory.Outcome:
                Balance += financialTransactionEntity.Amount;
                break;
        }

        financialTransactionEntity.SetReversed();
    }

    public void GenerateAccountNumber()
    {
        var accountNumber = NumberUtils.RandomNumber(1000, 9999).ToString("D5");
        AccountNumber = string.Concat(accountNumber, "-0");
    }
}