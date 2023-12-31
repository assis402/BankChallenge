using BankChallenge.Business.Entities;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Shared.Helpers;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Infrasctructure.Repositories;

public class AccountRepository(BankChallengeContextDb context) : BaseRepository<AccountEntity>(context), IAccountRepository
{
    public async Task<AccountEntity> FindByAccountNumber(string accountNumber)
    {
        var filterDefinition = Builders<AccountEntity>.Filter
            .Eq("accountNumber", accountNumber);

        return await FindOneAsync(filterDefinition);
    }

    public async Task<AccountEntity> FindByAccountNumber(string accountNumber, IClientSessionHandle session)
    {
        var filterDefinition = Builders<AccountEntity>.Filter
            .Eq("accountNumber", accountNumber);

        return await FindOneAsync(filterDefinition, session);
    }

    public async Task<IEnumerable<AccountEntity>> FindManyByAccountHolderId(string accountHolderId)
    {
        var filterDefinition = Builders<AccountEntity>.Filter
            .Eq("accountHolderId", accountHolderId);

        return await FindAsync(filterDefinition);
    }

    public async Task UpdateOneAsync(AccountEntity entity, IClientSessionHandle session)
    {
        var updateDefinition = Builders<AccountEntity>.Update
            .Set(nameof(entity.Balance).FirstCharToLowerCase(), entity.Balance);

        await UpdateOneAsync(entity, updateDefinition, session);
    }

    public async Task UpdateManyAsync(IClientSessionHandle session, params AccountEntity[] entityList)
    {
        foreach (var entity in entityList)
        {
            var updateDefinition = Builders<AccountEntity>.Update
                .Set(nameof(entity.Balance).FirstCharToLowerCase(), entity.Balance);

            await base.UpdateOneAsync(entity, updateDefinition, session);
        }
    }

    public async Task<bool> Exists(string accountNumber, IClientSessionHandle session)
    {
        var filter = Builders<AccountEntity>.Filter.Eq("accountNumber", accountNumber);
        return await Exists(filter, session);
    }
}