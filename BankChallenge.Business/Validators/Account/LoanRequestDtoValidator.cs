using BankChallenge.Business.Enums.Messages;
using BankChallenge.Shared.Dtos.Account;
using FluentValidation;
using Matsoft.ApiResults.Helpers;

namespace BankChallenge.Business.Validators.Account;

public class LoanRequestDtoValidator : BaseTransactionRequestDtoValidator<LoanRequestDto>
{
    public LoanRequestDtoValidator()
    {
        RuleFor(x => x.Amount)
            .LessThan(10_000)
            .WithMessage(BankChallengeError.RequestLoan_Validation_InvalidAmount.Description());

        RuleFor(x => x.PaymentDate)
            .NotEmpty()
            .Must(date => date > DateOnly.FromDateTime(DateTime.Now))
            .WithMessage(BankChallengeError.RequestLoan_Validation_InvalidPaymentDate.Description());
    }
}