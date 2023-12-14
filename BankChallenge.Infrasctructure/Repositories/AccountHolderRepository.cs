using BankChallenge.Business.Entities;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Shared.Dtos.Identity;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Infrasctructure.Repositories;

public class AccountHolderRepository(BankChallengeContextDb context) : BaseRepository<AccountHolderEntity>(context), IAccountHolderRepository
{
    public async Task<AccountHolderEntity> SignIn(string cpf, string password)
    {
        var filter = Builders<AccountHolderEntity>.Filter.Where(x =>
            x.Cpf.Equals(cpf) &&
            x.Password.Equals(password));

        return await FindOneAsync(filter);
    }
    
    public async Task<bool> Exists(SignUpDto request)
    {
        var filter = Builders<AccountHolderEntity>.Filter.Where(x =>
            x.Cpf.Equals(request.Cpf) || 
            x.Email.Equals(request.Email));
        
        return await Exists(filter);
    }
}