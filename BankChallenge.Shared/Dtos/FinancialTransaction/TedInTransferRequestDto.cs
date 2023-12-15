namespace BankChallenge.Shared.Dtos.FinancialTransaction;

public record TedInTransferRequestDto(string AccountNumber, decimal Amount, string DestinationAccountNumber)
    : BaseTransactionRequestDto(AccountNumber, Amount);