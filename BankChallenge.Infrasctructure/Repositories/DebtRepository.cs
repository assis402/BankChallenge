using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Shared.Helpers;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Infrasctructure.Repositories;

public class DebtRepository(BankChallengeContextDb context) : BaseRepository<DebtEntity>(context), IDebtRepository
{
    public async Task UpdateOneAsync(DebtEntity entity)
    {
        var updateDefinition = Builders<DebtEntity>.Update
            .Set(nameof(entity.Status).FirstCharToLowerCase(), entity.Status)
            .Set(nameof(entity.CurrentAmountToPay).FirstCharToLowerCase(), entity.CurrentAmountToPay);

        await base.UpdateOneAsync(entity, updateDefinition);
    }

    public async Task<DebtEntity> FindByTypeAndAccountHolderId(DebtType type, string accountHolderId)
    {
        var filterDefinition = Builders<DebtEntity>.Filter
            .Where(x =>
                x.AccountHolderId.Equals(accountHolderId) &&
                x.Type.Equals(type) &&
                !x.Status.Equals(DebtStatus.Paid));

        return await FindOneAsync(filterDefinition);
    }
}