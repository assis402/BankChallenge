using BankChallenge.Business.Entities;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IAccountRepository : IBaseRepository<AccountEntity>
{
    public Task<AccountEntity> FindByAccountNumber(string accountNumber);

    public Task<AccountEntity> FindByAccountNumber(string accountNumber, IClientSessionHandle session);

    public Task UpdateOneAsync(AccountEntity entity, IClientSessionHandle session);

    public Task UpdateManyAsync(IClientSessionHandle session, params AccountEntity[] entityList);

    public Task<IEnumerable<AccountEntity>> FindManyByAccountHolderId(string accountHolderId);

    public Task<bool> Exists(string accountNumber, IClientSessionHandle session);
}