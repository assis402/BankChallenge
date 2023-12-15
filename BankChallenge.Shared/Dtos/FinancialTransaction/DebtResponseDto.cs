using BankChallenge.Shared.Dtos.Account;

namespace BankChallenge.Shared.Dtos.FinancialTransaction;

public record struct DebtResponseDto(DebtDto Debt, AccountDto Account);