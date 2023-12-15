namespace BankChallenge.Shared.Dtos.FinancialTransaction;

public record LoanRequestDto(string AccountNumber, decimal Amount)
    : BaseTransactionRequestDto(AccountNumber, Amount);