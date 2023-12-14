using BankChallenge.Business.Enums;
using BankChallenge.Shared.Dtos.Account;
using Matsoft.MongoDB;
using Matsoft.MongoDB.CustomAttributes;

namespace BankChallenge.Business.Entities;

[CollectionName("financialTransaction")]
public class FinancialTransactionEntity : BaseEntity
{
    public FinancialTransactionEntity(FulfillDebtRequestDto request, DebtEntity debtEntity)
    {
        AccountId = request.AccountNumber;
        Amount = request.Amount;
        Category = FinancialTransactionCategory.Outcome;
        Type = debtEntity.GetFinancialTransactionTypeByRequest(request);
        Status = FinancialTransactionStatus.Pending;
    }

    public FinancialTransactionEntity(string accountId, decimal amount, FinancialTransactionType type, FinancialTransactionCategory category)
    {
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Category = category;
        Status = FinancialTransactionStatus.Pending;
    }

    public string AccountId { get; set; }
    
    public decimal Amount { get; set; }
    
    public FinancialTransactionType Type { get; set; }
    
    public FinancialTransactionStatus Status { get; set; }
    
    public FinancialTransactionCategory Category { get; set; }
    
    public void SetCompleted() => Status = FinancialTransactionStatus.Completed;

    public void SetInProcess() => Status = FinancialTransactionStatus.InProcess;

    public void SetReversed() => Status = FinancialTransactionStatus.Reversed;
}