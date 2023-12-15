using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums.Messages;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Shared.Dtos.Account;
using Matsoft.ApiResults;
using MongoDB.Driver;

namespace BankChallenge.Business.Services;

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    public async Task<AccountEntity> CreateCheckingAccount(string accountHolderId, IClientSessionHandle session)
    {
        var checkingAccount = new AccountEntity(accountHolderId);
        bool existsAccountWithSameNumber;

        do
        {
            checkingAccount.GenerateAccountNumber();
            existsAccountWithSameNumber = await accountRepository.Exists(checkingAccount.AccountHolderId, session);
        } while (existsAccountWithSameNumber);

        await accountRepository.InsertOneAsync(checkingAccount, session);

        return checkingAccount;
    }

    public async Task<ApiResult> GetAllAccountsByAccountHolderId(string accountHolderId)
    {
        var accounts = await accountRepository.FindManyByAccountHolderId(accountHolderId);

        if (!accounts?.Any() ?? true)
            return Result.Error(BankChallengeError.Application_Error_NotFound);

        return Result.Success(accounts.Select(account => (AccountDto)account));
    }
}