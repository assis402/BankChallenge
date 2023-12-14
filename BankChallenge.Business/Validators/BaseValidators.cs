using BankChallenge.Business.Validators.Account;
using BankChallenge.Shared.Dtos.Account;

namespace BankChallenge.Business.Validators;

public static class BaseValidators
{
    public static BaseTransactionRequestDtoValidator<BaseTransactionRequestDto> Transaction
        => new BaseTransactionRequestDtoValidator<BaseTransactionRequestDto>();
}