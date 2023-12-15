namespace BankChallenge.Shared.Dtos.FinancialTransaction;

public record struct FinancialTransactionDto(
    string Id,
    string AccountId,
    decimal Amount,
    string Type,
    string Status,
    string Category);