using BankChallenge.Business.Enums;
using BankChallenge.Shared.Dtos.Account;
using Matsoft.MongoDB;
using Matsoft.MongoDB.CustomAttributes;

namespace BankChallenge.Business.Entities;

[CollectionName("debt")]
public class DebtEntity : BaseEntity
{
    public decimal OriginalAmount { get; set; }
    
    public decimal AmountToPay { get; set; }
    
    public DateOnly DueDate { get; set; }
    
    public DebtStatus Status { get; set; }
    
    public bool IsOverdue() => DateOnly.FromDateTime(DateTime.Now) >= DueDate;
    
    public bool IsSettled() => Status is DebtStatus.Paid;

    public FinancialTransactionType GetFinancialTransactionTypeByRequest(FulfillDebtRequestDto requestDto)
        => requestDto.Amount >= AmountToPay ? FinancialTransactionType.FulfillDebt : FinancialTransactionType.PartiallyPayDebt;
    
    public bool Pay(FinancialTransactionEntity financialTransaction)
    {
        var success = false;

        if (financialTransaction.Status is not FinancialTransactionStatus.InProcess)
            return success;
        
        switch (financialTransaction.Type)
        {
            case FinancialTransactionType.PartiallyPayDebt:
                AmountToPay -= financialTransaction.Amount;
                Status = DebtStatus.PartiallyPaid;
                success = true;
                break;
            case FinancialTransactionType.FulfillDebt:
                AmountToPay = 0;
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