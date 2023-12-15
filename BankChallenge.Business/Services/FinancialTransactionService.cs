using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using BankChallenge.Business.Enums.Messages;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Business.Validators;
using BankChallenge.Business.Validators.FinancialTransaction;
using BankChallenge.Shared.Dtos.Account;
using BankChallenge.Shared.Dtos.FinancialTransaction;
using Matsoft.ApiResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BankChallenge.Business.Services;

public class FinancialTransactionService(
    IUnitOfWork unitOfWork,
    IAccountRepository accountRepository,
    IDebtRepository debtRepository,
    IFinancialTransactionRepository financialTransactionRepository) : IFinancialTransactionService
{
    public async Task<ApiResult> Withdraw(BaseTransactionRequestDto request, string accountHolderId)
    {
        using var uowSession = await unitOfWork.StartSessionAsync();

        uowSession.StartTransaction();

        try
        {
            var requestValidation = await BaseValidators.Transaction.ValidateAsync(request);

            if (!requestValidation.IsValid)
                return Result.Error(
                    BankChallengeError.Application_Error_InvalidRequest,
                    requestValidation.Errors);

            var (account, accountValidationError) = await FindOriginAccountByNumber(request.AccountNumber, accountHolderId, uowSession);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            if (!account.HasSufficientBalance(request.Amount))
                return Result.Error(BankChallengeError.Account_Validation_InsufficientBalance);

            var financialTransaction = await GenerateWithdrawFinancialTransaction(request, account.Id.ToString(), uowSession);

            account.ExecuteFinancialTransaction(financialTransaction);
            await accountRepository.UpdateOneAsync(account, uowSession);

            financialTransaction.SetCompleted();
            await financialTransactionRepository.UpdateManyAsync(uowSession, financialTransaction);

            await uowSession.CommitTransactionAsync();

            return Result.Success(BankChallengeMessage.Transaction_Success, (AccountDto)account);
        }
        catch (Exception ex)
        {
            await uowSession.AbortTransactionAsync();
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
    }

    public async Task<ApiResult> Deposit(BaseTransactionRequestDto request, string accountHolderId)
    {
        using var uowSession = await unitOfWork.StartSessionAsync();

        uowSession.StartTransaction();

        try
        {
            var requestValidation = await BaseValidators.Transaction.ValidateAsync(request);

            if (!requestValidation.IsValid)
                return Result.Error(
                    BankChallengeError.Application_Error_InvalidRequest,
                    requestValidation.Errors);

            var (account, accountValidationError) = await FindOriginAccountByNumber(request.AccountNumber, accountHolderId, uowSession);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            await Deposit(request.Amount, account, uowSession);

            await uowSession.CommitTransactionAsync();

            return Result.Success(BankChallengeMessage.Transaction_Success, (AccountDto)account);
        }
        catch (Exception ex)
        {
            await uowSession.AbortTransactionAsync();
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
    }

    public async Task Deposit(decimal amount, AccountEntity account, IClientSessionHandle session)
    {
        var financialTransaction = await GenerateDepositFinancialTransactions(account.Id.ToString(), amount, session);

        account.ExecuteFinancialTransaction(financialTransaction);
        await accountRepository.UpdateOneAsync(account, session);

        financialTransaction.SetCompleted();
        await financialTransactionRepository.UpdateOneAsync(financialTransaction, session);
    }

    public async Task<ApiResult> TedInTransfer(TedInTransferRequestDto request, string accountHolderId)
    {
        using var uowSession = await unitOfWork.StartSessionAsync();

        uowSession.StartTransaction();

        try
        {
            var requestValidation = await new TedTransferRequestDtoValidator().ValidateAsync(request);

            if (!requestValidation.IsValid)
                return Result.Error(
                    BankChallengeError.Application_Error_InvalidRequest,
                    requestValidation.Errors);

            var (fromAccount, accountValidationError) = await FindOriginAccountByNumber(request.AccountNumber, accountHolderId, uowSession);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            (var toAccount, accountValidationError) = await FindDestinationAccountByNumber(request.DestinationAccountNumber, uowSession);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            var (originFinancialTransaction, destinationFinancialTransaction) =
                await GenerateTedFinancialTransactions(request, fromAccount, toAccount, uowSession);

            fromAccount.ExecuteFinancialTransaction(originFinancialTransaction);
            toAccount.ExecuteFinancialTransaction(destinationFinancialTransaction);

            await accountRepository.UpdateManyAsync(uowSession, fromAccount, toAccount);

            originFinancialTransaction.SetCompleted();
            destinationFinancialTransaction.SetCompleted();

            await financialTransactionRepository.UpdateManyAsync(
                uowSession,
                originFinancialTransaction,
                destinationFinancialTransaction);

            await uowSession.CommitTransactionAsync();

            return Result.Success(BankChallengeMessage.Transaction_Success, (AccountDto)fromAccount);
        }
        catch (Exception ex)
        {
            await uowSession.AbortTransactionAsync();
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
    }

    public async Task<ApiResult> PayOffDebt(PayOffDebtRequestDto request, string accountHolderId)
    {
        using var uowSession = await unitOfWork.StartSessionAsync();

        uowSession.StartTransaction();

        try
        {
            var requestValidation = await new PayOffDebtRequestDtoValidator().ValidateAsync(request);

            if (!requestValidation.IsValid)
                return Result.Error(
                    BankChallengeError.Application_Error_InvalidRequest,
                    requestValidation.Errors);

            var (account, accountValidationError) = await FindOriginAccountByNumber(request.AccountNumber, accountHolderId, uowSession);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            var debt = await debtRepository.FindByIdAsync(request.DebtId, uowSession);

            if (debt is null)
                return Result.Error(BankChallengeError.Debt_Error_NonExists);

            if (!debt.AccountId.Equals(account.Id.ToString()))
                return Result.Error(BankChallengeError.Debt_Error_InvalidAccountHolder);

            if (debt.IsSettled())
                return Result.Error(BankChallengeError.Debt_Validation_IsSettled);

            if (debt.IsOverdue())
                return Result.Error(BankChallengeError.Debt_Validation_IsOverdue);

            if (!account.HasSufficientBalance(request.Amount))
                return Result.Error(BankChallengeError.Account_Validation_InsufficientBalance);

            var financialTransaction = new FinancialTransactionEntity(request, debt);
            await financialTransactionRepository.InsertOneAsync(financialTransaction, uowSession);

            account.ExecuteFinancialTransaction(financialTransaction);

            var paymentResult = debt.Pay(financialTransaction);
            if (paymentResult)
            {
                await debtRepository.UpdateOneAsync(debt, uowSession);
                financialTransaction.SetCompleted();
            }
            else
            {
                account.ReverseFinancialTransaction(financialTransaction);
                financialTransaction.SetReversed();
            }

            await accountRepository.UpdateOneAsync(account, uowSession);
            await financialTransactionRepository.UpdateOneAsync(financialTransaction, uowSession);

            await uowSession.CommitTransactionAsync();

            var response = new DebtResponseDto(debt, account);

            return Result.Success(financialTransaction.Type is FinancialTransactionType.PayOffDebt
                ? BankChallengeMessage.PayOffDebt_Success
                : BankChallengeMessage.PartiallyPayDebt_Success, response);
        }
        catch (Exception ex)
        {
            await uowSession.AbortTransactionAsync();
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
    }

    public async Task<ActionResult> RequestLoan(LoanRequestDto request, string accountHolderId)
    {
        using var uowSession = await unitOfWork.StartSessionAsync();

        uowSession.StartTransaction();

        try
        {
            var requestValidation = await new LoanRequestDtoValidator().ValidateAsync(request);

            if (!requestValidation.IsValid)
                return Result.Error(
                    BankChallengeError.Application_Error_InvalidRequest,
                    requestValidation.Errors);

            var (account, accountValidationError) =
                await FindOriginAccountByNumber(request.AccountNumber, accountHolderId, uowSession);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            var debt = await debtRepository.FindByTypeAndAccountId(DebtType.Loan, account.Id.ToString(), uowSession);

            if (debt is not null)
                return Result.Error(BankChallengeError.RequestLoan_Validation_Limit);

            debt = new DebtEntity(request, account);
            await debtRepository.InsertOneAsync(debt, uowSession);

            var financialTransaction = new FinancialTransactionEntity(
                accountId: debt.AccountId,
                amount: debt.RequestedLoanAmount,
                category: FinancialTransactionCategory.Income,
                type: FinancialTransactionType.LoanDeposit);

            await financialTransactionRepository.InsertOneAsync(financialTransaction, uowSession);

            account.ExecuteFinancialTransaction(financialTransaction);
            await accountRepository.UpdateOneAsync(account, uowSession);

            financialTransaction.SetCompleted();
            await financialTransactionRepository.UpdateOneAsync(financialTransaction, uowSession);

            await uowSession.CommitTransactionAsync();

            return Result.Success(BankChallengeMessage.Transaction_Success, new DebtResponseDto(debt, account));
        }
        catch (Exception ex)
        {
            await uowSession.AbortTransactionAsync();
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
    }

    public async Task<ApiResult> GetAllDebtsByAccountNumber(string accountNumber, string accountHolderId)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return Result.Error(BankChallengeError.Account_Validation_AccountIdIsRequired);

        var (account, accountValidationError) =
            await FindOriginAccountByNumber(accountNumber, accountHolderId);

        if (accountValidationError is not null)
            return Result.Error(accountValidationError);

        var debts = await debtRepository.FindManyByAccountId(account.Id.ToString());

        if (!debts?.Any() ?? true)
            return Result.Error(BankChallengeError.Application_Error_NotFound);

        return Result.Success(debts.Select(debt => (DebtDto)debt));
    }

    public async Task<ApiResult> GetAllTransactionsByAccountNumber(string accountNumber, string accountHolderId)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return Result.Error(BankChallengeError.Account_Validation_AccountIdIsRequired);

        var (account, accountValidationError) =
            await FindOriginAccountByNumber(accountNumber, accountHolderId);

        if (accountValidationError is not null)
            return Result.Error(accountValidationError);

        var financialTransactions = await financialTransactionRepository.FindManyByAccountId(account.Id.ToString());

        if (!financialTransactions?.Any() ?? true)
            return Result.Error(BankChallengeError.Application_Error_NotFound);

        return Result.Success(financialTransactions.Select(transaction => (FinancialTransactionDto)transaction));
    }

    private async Task<(AccountEntity, BankChallengeError?)> FindOriginAccountByNumber(string accountNumber, string accountHolderId)
    {
        var account = await accountRepository.FindByAccountNumber(accountNumber);

        if (account is null)
            return (null, BankChallengeError.Account_Origin_Error_NonExists);

        if (!account.IsActive())
            return (null, BankChallengeError.Account_Origin_Validation_Inactive);

        if (!accountHolderId.Equals(account.AccountHolderId))
            return (null, BankChallengeError.Account_Error_ForbiddenOperation);

        return (account, null);
    }

    private async Task<(AccountEntity, BankChallengeError?)> FindOriginAccountByNumber(string accountNumber, string accountHolderId, IClientSessionHandle session)
    {
        var account = await accountRepository.FindByAccountNumber(accountNumber, session);

        if (account is null)
            return (null, BankChallengeError.Account_Origin_Error_NonExists);

        if (!account.IsActive())
            return (null, BankChallengeError.Account_Origin_Validation_Inactive);

        if (!accountHolderId.Equals(account.AccountHolderId))
            return (null, BankChallengeError.Account_Error_ForbiddenOperation);

        return (account, null);
    }

    private async Task<(AccountEntity, BankChallengeError?)> FindDestinationAccountByNumber(string accountNumber, IClientSessionHandle session)
    {
        var account = await accountRepository.FindByAccountNumber(accountNumber, session);

        if (account is null)
            return (null, BankChallengeError.Account_Destination_Error_NonExists);

        if (!account.IsActive())
            return (null, BankChallengeError.Account_Destination_Validation_Inactive);

        return (account, null);
    }

    private async Task<FinancialTransactionEntity> GenerateDepositFinancialTransactions(string accountId, decimal amount, IClientSessionHandle session)
    {
        var financialTransaction = new FinancialTransactionEntity(
            accountId,
            amount,
            FinancialTransactionType.Deposit,
            FinancialTransactionCategory.Income);

        await financialTransactionRepository.InsertOneAsync(financialTransaction, session);

        return financialTransaction;
    }

    private async Task<FinancialTransactionEntity> GenerateWithdrawFinancialTransaction(BaseTransactionRequestDto request, string originAccountId, IClientSessionHandle session)
    {
        var financialTransaction = new FinancialTransactionEntity(
            originAccountId,
            request.Amount,
            FinancialTransactionType.Withdraw,
            FinancialTransactionCategory.Outcome);

        await financialTransactionRepository.InsertOneAsync(financialTransaction, session);

        return financialTransaction;
    }

    private async Task<(FinancialTransactionEntity, FinancialTransactionEntity)> GenerateTedFinancialTransactions(
        TedInTransferRequestDto request,
        AccountEntity fromAccount,
        AccountEntity toAccount,
        IClientSessionHandle session)
    {
        var originFinancialTransaction = new FinancialTransactionEntity(
            fromAccount.Id.ToString(),
            request.Amount,
            FinancialTransactionType.TedIn,
            FinancialTransactionCategory.Outcome);

        var destinationFinancialTransaction = new FinancialTransactionEntity(
            toAccount.Id.ToString(),
            request.Amount,
            FinancialTransactionType.TedIn,
            FinancialTransactionCategory.Income);

        await financialTransactionRepository.InsertManyAsync(session, originFinancialTransaction, destinationFinancialTransaction);

        return (originFinancialTransaction, destinationFinancialTransaction);
    }
}