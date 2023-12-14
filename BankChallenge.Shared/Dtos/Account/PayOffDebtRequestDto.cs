namespace BankChallenge.Shared.Dtos.Account;

public record PayOffDebtRequestDto(string DebtId, string AccountNumber, decimal Amount)
    : BaseTransactionRequestDto(AccountNumber, Amount);