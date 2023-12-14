namespace BankChallenge.Shared.Dtos.Account;

public record LoanRequestDto(string AccountNumber, decimal Amount, DateOnly PaymentDate) 
    : BaseTransactionRequestDto(AccountNumber, Amount);