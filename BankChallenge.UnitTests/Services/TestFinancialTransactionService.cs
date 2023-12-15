using BankChallenge.Business.Enums.Messages;
using BankChallenge.Business.Services;
using BankChallenge.UnitTests.Builders.Account;
using BankChallenge.UnitTests.Builders.FinancialTransaction;
using BankChallenge.UnitTests.Mocks.Repositories;
using Matsoft.ApiResults.Helpers;

namespace BankChallenge.UnitTests.Services;

public class TestFinancialTransactionService : BaseTest
{
    private const string ACCOUNT_HOLDER_ID = "657b416c75625452f403c6e7";

    #region PayOffDebt

    [Fact]
    public async Task PayOffDebt_ReturnsSuccessResult()
    {
        // Arrange
        var request = new PayOffDebtRequestDtoBuilder().Default().Build();

        var account = new AccountEntityBuilder().Default(ACCOUNT_HOLDER_ID).Build();

        var debt = new DebtEntityBuilder()
            .Default()
            .WithAccountId(account.Id.ToString())
            .WithCurrentAmountToPay(request.Amount)
            .Build();

        MockAccountRepository.MockFindOneAsync(account);
        MockFinancialTransactionRepository.MockInsertOneAsync();
        MockAccountRepository.MockUpdateOneAsync();
        MockDebtRepository.MockFindByIdAsync(debt);
        MockDebtRepository.MockUpdateOneAsync();
        MockFinancialTransactionRepository.MockUpdateOneAsync();

        // Act
        var result = await FinancialTransactionService.PayOffDebt(request, ACCOUNT_HOLDER_ID);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(BankChallengeMessage.PayOffDebt_Success.Description(), result.Message);
    }

    [Fact]
    public async Task PayOffDebt_InvalidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new PayOffDebtRequestDtoBuilder().Invalid().Build();

        // Act
        var result = await FinancialTransactionService.PayOffDebt(request, ACCOUNT_HOLDER_ID);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(BankChallengeError.Application_Error_InvalidRequest.Description(), result.Message);
    }

    [Fact]
    public async Task PayOffDebt_NonExistentDebit_ReturnsErrorResult()
    {
        // Arrange
        var request = new PayOffDebtRequestDtoBuilder().Default().Build();

        var account = new AccountEntityBuilder().Default(ACCOUNT_HOLDER_ID).Build();

        MockAccountRepository.MockFindOneAsync(account);
        MockFinancialTransactionRepository.MockInsertOneAsync();
        MockAccountRepository.MockUpdateOneAsync();
        MockDebtRepository.MockFindByIdAsync(null);

        // Act
        var result = await FinancialTransactionService.PayOffDebt(request, ACCOUNT_HOLDER_ID);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(BankChallengeError.Debt_Error_NonExists.Description(), result.Message);
    }

    #endregion PayOffDebt

    #region Withdraw

    [Fact]
    public async Task Withdraw_ReturnsSuccessResult()
    {
        // Arrange
        var request = new BaseTransactionRequestDtoBuilder().Default().Build();

        var account = new AccountEntityBuilder().Default(ACCOUNT_HOLDER_ID).Build();

        MockAccountRepository.MockFindOneAsync(account);
        MockFinancialTransactionRepository.MockInsertOneAsync();
        MockAccountRepository.MockUpdateOneAsync();
        MockFinancialTransactionRepository.MockUpdateOneAsync();

        // Act
        var result = await FinancialTransactionService.Withdraw(request, ACCOUNT_HOLDER_ID);

        // Assert
        MockAccountRepository.VerifyAll();
        MockFinancialTransactionRepository.VerifyAll();

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(BankChallengeMessage.Transaction_Success.Description(), result.Message);
    }

    [Fact]
    public async Task Withdraw_InvalidRequest_ReturnsErrorResult()
    {
        // Arrange
        var request = new BaseTransactionRequestDtoBuilder().Invalid().Build();

        // Act
        var result = await FinancialTransactionService.Withdraw(request, ACCOUNT_HOLDER_ID);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(BankChallengeError.Application_Error_InvalidRequest.Description(), result.Message);
    }

    [Fact]
    public async Task Withdraw_NonExistentAccount_ReturnsErrorResult()
    {
        // Arrange
        var request = new BaseTransactionRequestDtoBuilder().Default().Build();

        MockAccountRepository.MockFindOneAsync(null);

        // Act
        var result = await FinancialTransactionService.Withdraw(request, ACCOUNT_HOLDER_ID);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(BankChallengeError.Account_Origin_Error_NonExists.Description(), result.Message);
    }

    #endregion Withdraw

    #region Deposit

    [Fact]
    public async Task Deposit_ReturnsSuccessResult()
    {
        // Arrange
        var request = new BaseTransactionRequestDtoBuilder().Default().Build();

        var account = new AccountEntityBuilder().Default(ACCOUNT_HOLDER_ID).Build();

        MockAccountRepository.MockFindOneAsync(account);
        MockFinancialTransactionRepository.MockInsertOneAsync();
        MockAccountRepository.MockUpdateOneAsync();
        MockFinancialTransactionRepository.MockUpdateOneAsync();

        // Act
        var result = await FinancialTransactionService.Deposit(request, ACCOUNT_HOLDER_ID);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(BankChallengeMessage.Transaction_Success.Description(), result.Message);
    }

    [Fact]
    public async Task Deposit_InvalidRequest_ReturnsErrorResult()
    {
        // Arrange
        var request = new BaseTransactionRequestDtoBuilder().Invalid().Build();

        // Act
        var result = await FinancialTransactionService.Deposit(request, ACCOUNT_HOLDER_ID);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(BankChallengeError.Application_Error_InvalidRequest.Description(), result.Message);
    }

    [Fact]
    public async Task Deposit_NonExistentAccount_ReturnsErrorResult()
    {
        // Arrange
        var request = new BaseTransactionRequestDtoBuilder().Default().Build();

        MockAccountRepository.MockFindOneAsync(null);

        // Act
        var result = await FinancialTransactionService.Deposit(request, ACCOUNT_HOLDER_ID);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(BankChallengeError.Account_Origin_Error_NonExists.Description(), result.Message);
    }

    #endregion Deposit
}