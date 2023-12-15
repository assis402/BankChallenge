using BankChallenge.Business.Entities;
using BankChallenge.Shared.Dtos.Identity;
using Matsoft.MongoDB;
using MongoDB.Driver;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IAccountHolderRepository : IBaseRepository<AccountHolderEntity>
{
    public Task<AccountHolderEntity> SignIn(string cpf, string password);

    public Task<bool> Exists(SignUpDto request, IClientSessionHandle session);
}