using BankChallenge.Business.Validators.FinancialTransaction;
using BankChallenge.Shared.Dtos.FinancialTransaction;

namespace BankChallenge.Business.Validators;

public static class BaseValidators
{
    public static BaseTransactionRequestDtoValidator<BaseTransactionRequestDto> Transaction
        => new BaseTransactionRequestDtoValidator<BaseTransactionRequestDto>();
}