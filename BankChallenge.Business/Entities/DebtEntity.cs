using BankChallenge.Business.Enums;
using BankChallenge.Shared.Dtos.Account;
using Matsoft.MongoDB;
using Matsoft.MongoDB.CustomAttributes;

namespace BankChallenge.Business.Entities;

[CollectionName("debt")]
public class DebtEntity : BaseEntity
{
    public DebtEntity(LoanRequestDto request, AccountEntity accountEntity)
    {
        RequestedLoanAmount = OriginalAmountToPay = CurrentAmountToPay = request.Amount;
        AccountHolderId = accountEntity.AccountHolderId;
        DueDate = request.PaymentDate;
        Status = DebtStatus.Pending;
        Type = DebtType.Loan;

        ApplyLoanInterest();
    }

    public string AccountHolderId { get; private set; }

    public decimal OriginalAmountToPay { get; private set; }

    public decimal CurrentAmountToPay { get; private set; }

    public decimal? RequestedLoanAmount { get; private set; }

    public DateOnly DueDate { get; private set; }

    public DebtStatus Status { get; private set; }

    public DebtType Type { get; private set; }

    public bool IsOverdue() => DateOnly.FromDateTime(DateTime.Now) >= DueDate;

    public bool IsSettled() => Status is DebtStatus.Paid;

    private void ApplyLoanInterest()
    {
        const decimal interestRate = 5.0M; // Taxa de juros de 5% apenas de exemplo
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

            default:
                break;
        }

        return success;
    }
}