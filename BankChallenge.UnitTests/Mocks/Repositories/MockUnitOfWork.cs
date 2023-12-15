using BankChallenge.Business.Interfaces.Repositories;
using MongoDB.Driver;
using Moq;

namespace BankChallenge.UnitTests.Mocks.Repositories;

public class MockUnitOfWork : Mock<IUnitOfWork>
{
    public MockUnitOfWork() => MockStartSessionAsync();

    public void MockStartSessionAsync()
        => Setup(x => x.StartSessionAsync()).ReturnsAsync(new Mock<IClientSessionHandle>().Object);
}