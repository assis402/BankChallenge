using BankChallenge.Business.Entities;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Shared.Helpers;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Infrasctructure.Repositories;

public class FinancialTransactionRepository(BankChallengeContextDb context) : BaseRepository<FinancialTransactionEntity>(context), IFinancialTransactionRepository
{
    public async Task UpdateOneAsync(FinancialTransactionEntity entity)
    {
        var updateDefinition = Builders<FinancialTransactionEntity>.Update
            .Set(nameof(entity.Status).FirstCharToLowerCase(), entity.Status);

        await base.UpdateOneAsync(entity, updateDefinition);
    }
    
    public async Task UpdateManyAsync(params FinancialTransactionEntity[] entityList)
    {
        foreach (var entity in entityList)
        {
            var updateDefinition = Builders<FinancialTransactionEntity>.Update
                .Set(nameof(entity.Status).FirstCharToLowerCase(), entity.Status);

            await base.UpdateOneAsync(entity, updateDefinition);
        }
    }
}