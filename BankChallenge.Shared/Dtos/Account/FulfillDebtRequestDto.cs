namespace BankChallenge.Shared.Dtos.Account;

public record FulfillDebtRequestDto(string DebtId, string AccountNumber, decimal Amount) 
    : BaseTransactionRequestDto(AccountNumber, Amount);