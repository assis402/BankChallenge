namespace BankChallenge.Shared.Dtos.Account;

public record BaseTransactionRequestDto(string AccountNumber, decimal Amount);