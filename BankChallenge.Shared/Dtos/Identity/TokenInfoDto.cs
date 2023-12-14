namespace BankChallenge.Shared.Dtos.Identity;

public record struct TokenInfoDto(string AccessToken, int ExpiresIn);