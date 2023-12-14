namespace BankChallenge.Shared.Dtos.Account;

public record struct AccountDto(
    string AccountNumber,
    decimal Balance,
    string Type,
    string Status,
    DateOnly OpeningDate);