namespace BankChallenge.Shared.Dtos.FinancialTransaction;

public record PayOffDebtRequestDto(string DebtId, string AccountNumber, decimal Amount)
    : BaseTransactionRequestDto(AccountNumber, Amount);