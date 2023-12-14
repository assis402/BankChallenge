using BankChallenge.Business.Interfaces.Repositories;
using MongoDB.Driver;

namespace BankChallenge.Infrasctructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoClient _client;
    private IClientSessionHandle _session;

    public UnitOfWork(BankChallengeContextDb context) => _client = context.Client;

    public async Task<IClientSessionHandle> StartSessionAsync()
    {
        _session = await _client.StartSessionAsync();
        return _session;
    }

    public void Dispose() => _session.Dispose();
}