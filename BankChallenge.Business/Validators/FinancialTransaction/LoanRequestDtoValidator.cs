using BankChallenge.Business.Enums.Messages;
using BankChallenge.Shared.Dtos.FinancialTransaction;
using FluentValidation;
using Matsoft.ApiResults.Helpers;

namespace BankChallenge.Business.Validators.FinancialTransaction;

public class LoanRequestDtoValidator : BaseTransactionRequestDtoValidator<LoanRequestDto>
{
    public LoanRequestDtoValidator()
    {
        RuleFor(x => x.Amount)
            .LessThanOrEqualTo(20_000)
            .WithMessage(BankChallengeError.RequestLoan_Validation_InvalidAmount.Description());
    }
}