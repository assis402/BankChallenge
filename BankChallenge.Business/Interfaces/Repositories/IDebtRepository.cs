using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using Matsoft.MongoDB;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IDebtRepository : IBaseRepository<DebtEntity>
{
    public Task UpdateOneAsync(DebtEntity entity);
    
    public Task<DebtEntity> FindByTypeAndAccountHolderId(DebtType type, string accountHolderId);
}