using BankChallenge.Shared.Dtos.FinancialTransaction;
using FluentValidation;

namespace BankChallenge.Business.Validators.FinancialTransaction;

public class TedTransferRequestDtoValidator : BaseTransactionRequestDtoValidator<TedInTransferRequestDto>
{
    public TedTransferRequestDtoValidator()
    {
        RuleFor(x => x.DestinationAccountNumber)
            .NotEmpty();
    }
}