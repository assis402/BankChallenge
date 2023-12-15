using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Business.Services;
using BankChallenge.UnitTests.Mocks.Repositories;
using Moq;

namespace BankChallenge.UnitTests;

public class BaseTest
{
    #region Private Mocks

    private MockUnitOfWork _mockUnitOfWork;
    private MockAccountRepository _mockAccountRepository;
    private MockDebtRepository _mockDebtRepository;
    private MockFinancialTransactionRepository _mockFinancialTransactionRepository;

    #endregion Private Mocks

    #region Protected Mocks

    protected MockUnitOfWork MockUnitOfWork
        => _mockUnitOfWork ??= new MockUnitOfWork();

    protected MockAccountRepository MockAccountRepository
        => _mockAccountRepository ??= new MockAccountRepository();

    protected MockDebtRepository MockDebtRepository
        => _mockDebtRepository ??= new MockDebtRepository();

    protected MockFinancialTransactionRepository MockFinancialTransactionRepository
        => _mockFinancialTransactionRepository ??= new MockFinancialTransactionRepository();

    #endregion Protected Mocks

    #region Services

    protected FinancialTransactionService FinancialTransactionService => new(
        MockUnitOfWork.Object,
        MockAccountRepository.Object,
        MockDebtRepository.Object,
        MockFinancialTransactionRepository.Object);

    #endregion Services
}