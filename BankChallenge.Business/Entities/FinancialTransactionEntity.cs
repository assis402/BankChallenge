using BankChallenge.Business.Enums;
using BankChallenge.Shared.Dtos.FinancialTransaction;
using Matsoft.MongoDB;
using Matsoft.MongoDB.CustomAttributes;

namespace BankChallenge.Business.Entities;

[CollectionName("financialTransaction")]
public class FinancialTransactionEntity : BaseEntity
{
    public FinancialTransactionEntity(PayOffDebtRequestDto request, DebtEntity debtEntity)
    {
        AccountId = debtEntity.AccountId;
        Amount = request.Amount;
        Category = FinancialTransactionCategory.Outcome;
        Type = debtEntity.GetFinancialTransactionTypeByRequest(request);
        Status = FinancialTransactionStatus.Pending;
    }

    public FinancialTransactionEntity(string accountId, decimal amount, FinancialTransactionType type,
        FinancialTransactionCategory category)
    {
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Category = category;
        Status = FinancialTransactionStatus.Pending;
    }

    public static implicit operator FinancialTransactionDto(FinancialTransactionEntity entity)
        => new(entity.Id.ToString(),
            entity.AccountId,
            entity.Amount,
            entity.Type.ToString(),
            entity.Status.ToString(),
            entity.Category.ToString());

    public string AccountId { get; private set; }

    public decimal Amount { get; private set; }

    public FinancialTransactionType Type { get; private set; }

    public FinancialTransactionStatus Status { get; private set; }

    public FinancialTransactionCategory Category { get; private set; }

    public void SetCompleted() => Status = FinancialTransactionStatus.Completed;

    public void SetInProcess() => Status = FinancialTransactionStatus.InProcess;

    public void SetReversed() => Status = FinancialTransactionStatus.Reversed;
}