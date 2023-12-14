using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using BankChallenge.Business.Enums.Messages;
using BankChallenge.Business.Helpers;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Business.Validators.Identity;
using BankChallenge.Shared.Dtos.Account;
using BankChallenge.Shared.Dtos.Identity;
using Matsoft.ApiResults;

namespace BankChallenge.Business.Services;

public class AccountHolderService(
        IUnitOfWork unitOfWork,
        IAccountHolderRepository accountHolderRepository,
        IAccountService accountService,
        ITokenService tokenService) : IAccountHolderService
{
    public async Task<ApiResult> SignIn(SignInDto signInDto)
    {
        try
        {
            var signInValidation = await new SignInDtoValidator().ValidateAsync(signInDto);

            if (!signInValidation.IsValid)
                return Result.Error(
                    BankChallengeError.Application_Error_InvalidRequest,
                    signInValidation.Errors);

            var accountHolder = await accountHolderRepository.SignIn(signInDto.Cpf, CryptographyMd5.Encrypt(signInDto.Password));
            if (accountHolder is null)
                return Result.Error(BankChallengeError.SignIn_Error_WrongCpfOrPassword);

            if (accountHolder.Status is AccountHolderStatus.Inactive)
                return Result.Error(BankChallengeError.AccountHolder_Validation_Inactive);

            var tokenInfo = tokenService.GenerateTokenInfo(accountHolder);
            var signInResponse = new SignInResponseDto(accountHolder, tokenInfo);

            return Result.Success(BankChallengeMessage.SignIn_Success, signInResponse);
        }
        catch (Exception ex)
        {
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
    }

    public async Task<ApiResult> SignUp(SignUpDto request)
    {
        using var uowSession = await unitOfWork.StartSessionAsync();

        uowSession.StartTransaction();

        try
        {
            var validation = await new SignUpDtoValidator().ValidateAsync(request);

            if (!validation.IsValid)
                return Result.Error(
                    BankChallengeError.Application_Error_InvalidRequest,
                    validation.Errors);

            var exists = await accountHolderRepository.Exists(request);

            if (exists)
                return Result.Error(BankChallengeError.SignUp_Error_AccountHolderAlreadyExists);

            var accountHolder = new AccountHolderEntity(request);
            await accountHolderRepository.InsertOneAsync(accountHolder);
            var account = await accountService.CreateCheckingAccount(accountHolder.Id.ToString());

            if (request.InitialDeposit > 0)
                await accountService.Deposit(request.InitialDeposit, account);

            await uowSession.CommitTransactionAsync();

            return Result.Success(BankChallengeMessage.SignUp_Success, (AccountDto)account);
        }
        catch (Exception ex)
        {
            await uowSession.AbortTransactionAsync();
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
    }
}