using BankChallenge.Business.Enums.Messages;
using BankChallenge.Shared.Dtos.FinancialTransaction;
using FluentValidation;
using Matsoft.ApiResults.Helpers;

namespace BankChallenge.Business.Validators.FinancialTransaction;

public class BaseTransactionRequestDtoValidator<TRequest> : AbstractValidator<TRequest> where TRequest : BaseTransactionRequestDto
{
    public BaseTransactionRequestDtoValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(BankChallengeError.Transaction_Validation_InsufficientAmount.Description());
    }
}