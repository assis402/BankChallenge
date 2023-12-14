namespace BankChallenge.Shared.Dtos.Identity;

public record SignUpDto(
    string Email,
    string Password,
    string Cpf,
    string Name,
    DateOnly Birthdate,
    string Address,
    decimal InitialDeposit);
