using BankChallenge.Business.Entities;
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
            .Set(nameof(entity.AmountToPay).FirstCharToLowerCase(), entity.AmountToPay);

        await base.UpdateOneAsync(entity, updateDefinition);
    }
}