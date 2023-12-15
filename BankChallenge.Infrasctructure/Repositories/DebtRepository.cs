using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Shared.Helpers;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Infrasctructure.Repositories;

public class DebtRepository(BankChallengeContextDb context) : BaseRepository<DebtEntity>(context), IDebtRepository
{
    public async Task UpdateOneAsync(DebtEntity entity, IClientSessionHandle session)
    {
        var updateDefinition = Builders<DebtEntity>.Update
            .Set(nameof(entity.Status).FirstCharToLowerCase(), entity.Status)
            .Set(nameof(entity.CurrentAmountToPay).FirstCharToLowerCase(), entity.CurrentAmountToPay);

        await UpdateOneAsync(entity, updateDefinition, session);
    }

    public async Task<DebtEntity> FindByTypeAndAccountId(DebtType type, string accountId, IClientSessionHandle session)
    {
        var filterDefinition = Builders<DebtEntity>.Filter
            .Where(x =>
                x.AccountId.Equals(accountId) &&
                x.Type == type &&
                x.Status != DebtStatus.Paid);

        return await FindOneAsync(filterDefinition, session);
    }

    public async Task<IEnumerable<DebtEntity>> FindManyByAccountId(string accountId)
    {
        var filterDefinition = Builders<DebtEntity>.Filter
            .Eq(x => x.AccountId, accountId);

        return await FindAsync(filterDefinition);
    }
}