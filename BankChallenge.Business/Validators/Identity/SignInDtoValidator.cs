using BankChallenge.Shared.Dtos.Identity;
using FluentValidation;

namespace BankChallenge.Business.Validators.Identity;

public class SignInDtoValidator : AbstractValidator<SignInDto>
{
    public SignInDtoValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x.Cpf)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}