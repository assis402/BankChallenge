using BankChallenge.Business.Entities;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IFinancialTransactionRepository : IBaseRepository<FinancialTransactionEntity>
{
    public Task UpdateOneAsync(FinancialTransactionEntity entity, IClientSessionHandle session);

    public Task UpdateManyAsync(IClientSessionHandle session, params FinancialTransactionEntity[] entityList);

    public Task<IEnumerable<FinancialTransactionEntity>> FindManyByAccountId(string accountId);
}