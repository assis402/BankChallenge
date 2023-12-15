namespace BankChallenge.Shared.Dtos.FinancialTransaction;

public record BaseTransactionRequestDto(string AccountNumber, decimal Amount);