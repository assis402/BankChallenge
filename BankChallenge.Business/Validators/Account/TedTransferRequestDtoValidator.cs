using BankChallenge.Shared.Dtos.Account;
using FluentValidation;

namespace BankChallenge.Business.Validators.Account;

public class TedTransferRequestDtoValidator : BaseTransactionRequestDtoValidator<TedInTransferRequestDto>
{
    public TedTransferRequestDtoValidator()
    {
        RuleFor(x => x.DestinationAccountNumber)
            .NotEmpty();
    }
}