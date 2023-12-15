using BankChallenge.Business.Enums.Messages;
using BankChallenge.Shared.Dtos.Identity;
using BankChallenge.Shared.Helpers;
using FluentValidation;
using Matsoft.ApiResults.Helpers;

namespace BankChallenge.Business.Validators.Identity;

public class SignUpDtoValidator : AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage(BankChallengeError.SignUp_Validation_InvalidEmail.Description());

        RuleFor(x => x.Birthdate)
            .NotEmpty()
            .Must(BeOver18YearsOld)
            .WithMessage(BankChallengeError.SignUp_Validation_InvalidAge.Description());

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .ValidCpf()
            .WithMessage(BankChallengeError.SignUp_Validation_InvalidCpf.Description());

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])")
            .WithMessage(BankChallengeError.SignUp_Validation_InvalidPasswordRules.Description());

        RuleFor(x => x.InitialDeposit)
            .Must(x => x <= 100_000)
            .When(x => x.InitialDeposit > 0)
            .WithMessage(BankChallengeError.SignUp_Validation_InvalidInitialDeposit.Description());
    }

    private static bool BeOver18YearsOld(DateOnly birthdate)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - birthdate.Year;

        if (today.DayOfYear < birthdate.DayOfYear) age--;

        return age >= 18;
    }
}