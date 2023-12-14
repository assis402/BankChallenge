using BankChallenge.Business.Entities;
using Matsoft.MongoDB;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IDebtRepository : IBaseRepository<DebtEntity>
{
    Task UpdateOneAsync(DebtEntity entity);
}