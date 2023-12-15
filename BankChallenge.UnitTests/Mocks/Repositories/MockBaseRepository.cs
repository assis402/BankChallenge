using BankChallenge.Infrasctructure;
using Matsoft.MongoDB;
using MongoDB.Driver;
using Moq;

namespace BankChallenge.UnitTests.Mocks.Repositories;

public abstract class MockBaseRepository<TRepository, TEntity> : Mock<TRepository>
    where TRepository : BaseRepository<TEntity>
    where TEntity : BaseEntity
{
    public MockBaseRepository() : base(new BankChallengeContextDb(isTestProject: true)) => CallBase = true;

    internal void MockInsertOneAsync()
        => Setup(x => x.InsertOneAsync(It.IsAny<TEntity>(), It.IsAny<IClientSessionHandle>())).Returns(Task.CompletedTask);

    internal void MockInsertManyAsync()
        => Setup(x => x.InsertManyAsync(It.IsAny<IClientSessionHandle>(), It.IsAny<TEntity[]>())).Returns(Task.CompletedTask);

    internal void MockFindByIdAsync(TEntity entity)
        => Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<IClientSessionHandle>())).ReturnsAsync(entity);

    internal void MockFindAsync(IEnumerable<TEntity> entities)
        => Setup(x => x.FindAsync(It.IsAny<FilterDefinition<TEntity>>(), It.IsAny<IClientSessionHandle>())).ReturnsAsync(entities);

    internal void MockFindOneAsync(TEntity entity)
        => Setup(x => x.FindOneAsync(It.IsAny<FilterDefinition<TEntity>>(), It.IsAny<IClientSessionHandle>())).ReturnsAsync(entity);

    internal void MockExists(bool exists)
        => Setup(x => x.Exists(It.IsAny<FilterDefinition<TEntity>>(), It.IsAny<IClientSessionHandle>())).ReturnsAsync(exists);

    internal void MockUpdateOneAsync()
        => Setup(x => x.UpdateOneAsync(It.IsAny<TEntity>(), It.IsAny<UpdateDefinition<TEntity>>(), It.IsAny<IClientSessionHandle>()))
            .Returns(Task.CompletedTask);
}