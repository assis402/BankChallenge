using BankChallenge.Shared.Dtos.Account;
using FluentValidation;

namespace BankChallenge.Business.Validators.Account;

public class FulfillDebtRequestDtoValidator : BaseTransactionRequestDtoValidator<FulfillDebtRequestDto>
{
    public FulfillDebtRequestDtoValidator()
    {
        RuleFor(x => x.DebtId)
            .NotEmpty();
    }
}