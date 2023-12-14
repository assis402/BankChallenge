using MongoDB.Driver;

namespace BankChallenge.Business.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    public Task<IClientSessionHandle> StartSessionAsync();
}