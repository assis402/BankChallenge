using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IDebtRepository : IBaseRepository<DebtEntity>
{
    public Task UpdateOneAsync(DebtEntity entity, IClientSessionHandle session);

    public Task<DebtEntity> FindByTypeAndAccountId(DebtType type, string accountId, IClientSessionHandle session);

    public Task<IEnumerable<DebtEntity>> FindManyByAccountId(string accountId);
}