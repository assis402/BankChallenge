namespace BankChallenge.Shared.Dtos.FinancialTransaction;

public record struct DebtDto(
    string Id,
    decimal? RequestedLoanAmount,
    decimal OriginalAmountToPay,
    decimal CurrentAmountToPay,
    DateOnly DueDate,
    string Status,
    string Type);