namespace BankChallenge.Shared.Dtos.Account;

public record TedInTransferRequestDto(string AccountNumber, decimal Amount, string DestinationAccountNumber)
    : BaseTransactionRequestDto(AccountNumber, Amount);