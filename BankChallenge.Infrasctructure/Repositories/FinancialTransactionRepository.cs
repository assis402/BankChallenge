using BankChallenge.Business.Entities;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Shared.Helpers;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Infrasctructure.Repositories;

public class FinancialTransactionRepository(BankChallengeContextDb context) : BaseRepository<FinancialTransactionEntity>(context), IFinancialTransactionRepository
{
    public async Task UpdateOneAsync(FinancialTransactionEntity entity, IClientSessionHandle session)
    {
        var updateDefinition = Builders<FinancialTransactionEntity>.Update
            .Set(nameof(entity.Status).FirstCharToLowerCase(), entity.Status);

        await UpdateOneAsync(entity, updateDefinition, session);
    }

    public async Task UpdateManyAsync(IClientSessionHandle session, params FinancialTransactionEntity[] entityList)
    {
        foreach (var entity in entityList)
        {
            var updateDefinition = Builders<FinancialTransactionEntity>.Update
                .Set(nameof(entity.Status).FirstCharToLowerCase(), entity.Status);

            await UpdateOneAsync(entity, updateDefinition, session);
        }
    }

    public async Task<IEnumerable<FinancialTransactionEntity>> FindManyByAccountId(string accountId)
    {
        var filterDefinition = Builders<FinancialTransactionEntity>.Filter
            .Eq(x => x.AccountId, accountId);

        return await FindAsync(filterDefinition);
    }
}