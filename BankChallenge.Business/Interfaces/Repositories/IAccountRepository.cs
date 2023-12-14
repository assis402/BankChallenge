using BankChallenge.Business.Entities;
using Matsoft.MongoDB;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IAccountRepository : IBaseRepository<AccountEntity>
{
    public Task<AccountEntity> FindByAccountNumber(string accountNumber);
    
    public Task UpdateOneAsync(AccountEntity entity);
    
    public Task UpdateManyAsync(params AccountEntity[] entityList);
    
    public Task<IEnumerable<AccountEntity>> FindManyByAccountHolderId(string accountHolderId);
    
    public Task<bool> Exists(string accountNumber);
}