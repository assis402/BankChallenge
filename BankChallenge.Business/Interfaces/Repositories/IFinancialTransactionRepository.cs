using BankChallenge.Business.Entities;
using Matsoft.MongoDB;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IFinancialTransactionRepository : IBaseRepository<FinancialTransactionEntity>
{
    public Task UpdateOneAsync(FinancialTransactionEntity entity);

    public Task UpdateManyAsync(params FinancialTransactionEntity[] entityList);
}