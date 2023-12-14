using BankChallenge.Shared.Dtos.Account;
using FluentValidation;

namespace BankChallenge.Business.Validators.Account;

public class PayOffDebtRequestDtoValidator : BaseTransactionRequestDtoValidator<PayOffDebtRequestDto>
{
    public PayOffDebtRequestDtoValidator()
    {
        RuleFor(x => x.DebtId)
            .NotEmpty();
    }
}