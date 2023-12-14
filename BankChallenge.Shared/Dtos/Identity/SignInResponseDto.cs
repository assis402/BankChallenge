using BankChallenge.Shared.Dtos.AccountHolder;

namespace BankChallenge.Shared.Dtos.Identity;

public record struct SignInResponseDto(AccountHolderDto AccountHolder, TokenInfoDto TokenInfo);