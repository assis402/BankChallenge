using BankChallenge.Business.Entities;
using Matsoft.ApiResults;
using MongoDB.Driver;

namespace BankChallenge.Business.Interfaces.Services;

public interface IAccountService
{
    public Task<AccountEntity> CreateCheckingAccount(string accountHolderId, IClientSessionHandle session);

    public Task<ApiResult> GetAllAccountsByAccountHolderId(string accountHolderId);
}