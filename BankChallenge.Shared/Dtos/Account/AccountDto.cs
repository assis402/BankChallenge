namespace BankChallenge.Shared.Dtos.Account;

public record AccountDto(
    string AccountAgency,
    string AccountNumber,
    decimal Balance,
    string Type,
    string Status,
    DateOnly OpeningDate);