using BankChallenge.Business.Enums;
using BankChallenge.Shared.Dtos.FinancialTransaction;
using Matsoft.MongoDB;
using Matsoft.MongoDB.CustomAttributes;

namespace BankChallenge.Business.Entities;

[CollectionName("debt")]
public class DebtEntity : BaseEntity
{
    public DebtEntity()
    {
    }

    public DebtEntity(LoanRequestDto request, AccountEntity accountEntity)
    {
        RequestedLoanAmount = OriginalAmountToPay = CurrentAmountToPay = request.Amount;
        AccountId = accountEntity.Id.ToString();
        DueDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(1));
        Status = DebtStatus.Pending;
        Type = DebtType.Loan;

        ApplyLoanInterest();
    }

    public static implicit operator DebtDto(DebtEntity entity)
        => new(entity.Id.ToString(),
            entity.RequestedLoanAmount,
            entity.OriginalAmountToPay,
            entity.CurrentAmountToPay,
            entity.DueDate,
            entity.Status.ToString(),
            entity.Type.ToString());

    public string AccountId { get; set; }

    public decimal OriginalAmountToPay { get; set; }

    public decimal CurrentAmountToPay { get; set; }

    public decimal RequestedLoanAmount { get; set; }

    public DateOnly DueDate { get; set; }

    public DebtStatus Status { get; set; }

    public DebtType Type { get; set; }

    public bool IsOverdue() => DateOnly.FromDateTime(DateTime.Now) >= DueDate;

    public bool IsSettled() => Status is DebtStatus.Paid;

    /// <summary>
    /// Este método é uma representação simplificada de aplicação de juros em um empréstimo.
    /// </summary>
    private void ApplyLoanInterest()
    {
        const decimal interestRate = 5.00M; // Taxa de juros de 5% apenas como exemplo

        var interestAmount = OriginalAmountToPay * (interestRate / 100);

        OriginalAmountToPay += interestAmount;
        CurrentAmountToPay += interestAmount;
    }

    public FinancialTransactionType GetFinancialTransactionTypeByRequest(PayOffDebtRequestDto requestDto)
        => requestDto.Amount >= CurrentAmountToPay ? FinancialTransactionType.PayOffDebt : FinancialTransactionType.PartiallyPayDebt;

    public bool Pay(FinancialTransactionEntity financialTransaction)
    {
        var success = false;

        if (financialTransaction.Status is not FinancialTransactionStatus.InProcess)
            return success;

        switch (financialTransaction.Type)
        {
            case FinancialTransactionType.PartiallyPayDebt:
                CurrentAmountToPay -= financialTransaction.Amount;
                Status = DebtStatus.PartiallyPaid;
                success = true;
                break;

            case FinancialTransactionType.PayOffDebt:
                CurrentAmountToPay = 0;
                Status = DebtStatus.Paid;
                financialTransaction.SetCompleted();
                success = true;
                break;
        }

        return success;
    }
}