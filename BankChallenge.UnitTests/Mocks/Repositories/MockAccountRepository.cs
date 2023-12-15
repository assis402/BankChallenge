using BankChallenge.Business.Entities;
using BankChallenge.Infrasctructure.Repositories;

namespace BankChallenge.UnitTests.Mocks.Repositories;

public class MockAccountRepository : MockBaseRepository<AccountRepository, AccountEntity>;