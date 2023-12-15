using BankChallenge.Shared.Dtos.FinancialTransaction;
using FluentValidation;

namespace BankChallenge.Business.Validators.FinancialTransaction;

public class PayOffDebtRequestDtoValidator : BaseTransactionRequestDtoValidator<PayOffDebtRequestDto>
{
    public PayOffDebtRequestDtoValidator()
    {
        RuleFor(x => x.DebtId)
            .NotEmpty();
    }
}