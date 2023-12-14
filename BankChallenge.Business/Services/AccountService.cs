using BankChallenge.Business.Entities;
using BankChallenge.Business.Enums;
using BankChallenge.Business.Enums.Messages;
using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Business.Validators;
using BankChallenge.Business.Validators.Account;
using BankChallenge.Shared.Dtos.Account;
using Matsoft.ApiResults;
using Microsoft.AspNetCore.Mvc;

namespace BankChallenge.Business.Services;

public class AccountService(
    IUnitOfWork unitOfWork,
    IAccountRepository accountRepository,
    IDebtRepository debtRepository,
    IFinancialTransactionRepository financialTransactionRepository
) : IAccountService
{
    public async Task<AccountEntity> CreateCheckingAccount(string accountHolderId)
    {
        var checkingAccount = new AccountEntity(accountHolderId);
        bool existsAccountWithSameNumber;
        
        do
        {
            checkingAccount.GenerateAccountNumber();
            existsAccountWithSameNumber = await accountRepository.Exists(checkingAccount.AccountHolderId);
        } while (existsAccountWithSameNumber);

        await accountRepository.InsertOneAsync(checkingAccount);

        return checkingAccount;
    }

    public async Task<ApiResult> GetAllAccountsByAccountHolderId(string accountHolderId)
    {
        var accounts = await accountRepository.FindManyByAccountHolderId(accountHolderId);

        if (!accounts?.Any() ?? true)
            return Result.Error(BankChallengeError.Application_Error_NotFound);

        return Result.Success(accounts.Select(account => (AccountDto)account));
    }

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

            var (account, accountValidationError) = await FindOriginAccountByNumber(request.AccountNumber, accountHolderId);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            if (!account.HasSufficientBalance(request.Amount))
                return Result.Error(BankChallengeError.Account_Validation_InsufficientBalance);

            var financialTransaction = await GenerateWithdrawFinancialTransaction(request, account.Id.ToString());
            
            account.ExecuteFinancialTransaction(financialTransaction);
            await accountRepository.UpdateOneAsync(account);

            financialTransaction.SetCompleted();
            await financialTransactionRepository.UpdateManyAsync(financialTransaction);

            await uowSession.CommitTransactionAsync();

            return Result.Success(BankChallengeMessage.Transaction_Success);
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

            var (account, accountValidationError) = await FindOriginAccountByNumber(request.AccountNumber, accountHolderId);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            await Deposit(request.Amount, account);
            
            await uowSession.CommitTransactionAsync();

            return Result.Success(BankChallengeMessage.Transaction_Success);
        }
        catch (Exception ex)
        {
            await uowSession.AbortTransactionAsync();
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
    }

    public async Task Deposit(decimal amount, AccountEntity account)
    {
        var financialTransaction = await GenerateDepositFinancialTransactions(account.Id.ToString(), amount);
        
        account.ExecuteFinancialTransaction(financialTransaction);
        await accountRepository.UpdateOneAsync(account);

        financialTransaction.SetCompleted();
        await financialTransactionRepository.UpdateManyAsync(financialTransaction);
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

            if (!accountHolderId.Equals(request.AccountNumber))
                return Result.Error(BankChallengeError.Account_Error_ForbiddenOperation);

            var (fromAccount, accountValidationError) = await FindOriginAccountByNumber(request.AccountNumber, accountHolderId);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            (var toAccount, accountValidationError) = await FindDestinationAccountByNumber(request.DestinationAccountNumber);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            var (originFinancialTransaction, destinationFinancialTransaction) =
                await GenerateTedFinancialTransactions(request, fromAccount, toAccount);

            fromAccount.ExecuteFinancialTransaction(originFinancialTransaction);
            toAccount.ExecuteFinancialTransaction(destinationFinancialTransaction);

            await accountRepository.UpdateManyAsync(fromAccount, toAccount);

            originFinancialTransaction.SetCompleted();
            destinationFinancialTransaction.SetCompleted();

            await financialTransactionRepository.UpdateManyAsync(originFinancialTransaction,
                destinationFinancialTransaction);

            await uowSession.CommitTransactionAsync();

            return Result.Success(BankChallengeMessage.Transaction_Success);
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

            var (account, accountValidationError) = await FindOriginAccountByNumber(request.AccountNumber, accountHolderId);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);

            var debt = await debtRepository.FindByIdAsync(request.DebtId);

            if (debt is null)
                return Result.Error(BankChallengeError.Debt_Error_NonExists);
            
            if (!debt.AccountHolderId.Equals(accountHolderId))
                return Result.Error(BankChallengeError.Debt_Error_InvalidAccountHolder);
                
            if (debt.IsSettled())
                return Result.Error(BankChallengeError.Debt_Validation_IsSettled);

            if (debt.IsOverdue())
                return Result.Error(BankChallengeError.Debt_Validation_IsOverdue);

            if (!account.HasSufficientBalance(request.Amount))
                return Result.Error(BankChallengeError.Account_Validation_InsufficientBalance);

            var financialTransaction = new FinancialTransactionEntity(request, debt);
            await financialTransactionRepository.InsertOneAsync(financialTransaction);

            account.ExecuteFinancialTransaction(financialTransaction);

            var paymentResult = debt.Pay(financialTransaction);
            if (paymentResult)
            {
                await accountRepository.UpdateOneAsync(account);
                await debtRepository.UpdateOneAsync(debt);

                financialTransaction.SetCompleted();

                await financialTransactionRepository.UpdateOneAsync(financialTransaction);
            }
            else
            {
                account.ReverseFinancialTransaction(financialTransaction);
                financialTransaction.SetReversed();
            }

            await uowSession.CommitTransactionAsync();

            return Result.Success(financialTransaction.Type is FinancialTransactionType.PayOffDebt
                ? BankChallengeMessage.PayOffDebt_Success
                : BankChallengeMessage.PartiallyPayDebt_Success);
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
                await FindOriginAccountByNumber(request.AccountNumber, accountHolderId);

            if (accountValidationError is not null)
                return Result.Error(accountValidationError);
            
            var debt = await debtRepository.FindByTypeAndAccountHolderId(DebtType.Loan, accountHolderId);
            
            if (debt is not null)
                Result.Error(BankChallengeError.RequestLoan_Validation_Limit);

            debt = new DebtEntity(request, account);
            await debtRepository.InsertOneAsync(debt);
            
            var financialTransaction = new FinancialTransactionEntity(
                accountId: debt.AccountHolderId,
                amount: debt.CurrentAmountToPay,
                category: FinancialTransactionCategory.Income,
                type: FinancialTransactionType.LoanDeposit);
            
            await financialTransactionRepository.InsertOneAsync(financialTransaction);
            
            account.ExecuteFinancialTransaction(financialTransaction);
            await accountRepository.UpdateOneAsync(account);
            
            financialTransaction.SetCompleted();
            await financialTransactionRepository.UpdateOneAsync(financialTransaction);
            
            await uowSession.CommitTransactionAsync();
            
            return Result.Success(BankChallengeMessage.Transaction_Success);
        }
        catch (Exception ex)
        {
            await uowSession.AbortTransactionAsync();
            return Result.Error(BankChallengeError.Application_Error_General, ex);
        }
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

    private async Task<(AccountEntity, BankChallengeError?)> FindDestinationAccountByNumber(string accountNumber)
    {
        var account = await accountRepository.FindByAccountNumber(accountNumber);

        if (account is null)
            return (null, BankChallengeError.Account_Destination_Error_NonExists);

        if (!account.IsActive())
            return (null, BankChallengeError.Account_Destination_Validation_Inactive);

        return (account, null);
    }

    private async Task<FinancialTransactionEntity> GenerateDepositFinancialTransactions(string accountId, decimal amount) 
    {
        var financialTransaction = new FinancialTransactionEntity(
            accountId,
            amount,
            FinancialTransactionType.Deposit,
            FinancialTransactionCategory.Income);

        await financialTransactionRepository.InsertOneAsync(financialTransaction);

        return financialTransaction;
    }
    
    private async Task<FinancialTransactionEntity> GenerateWithdrawFinancialTransaction(BaseTransactionRequestDto request, string originAccountId) 
    {
        var financialTransaction = new FinancialTransactionEntity(
            originAccountId,
            request.Amount,
            FinancialTransactionType.Withdraw,
            FinancialTransactionCategory.Outcome);

        await financialTransactionRepository.InsertOneAsync(financialTransaction);

        return financialTransaction;
    }

    private async Task<(FinancialTransactionEntity, FinancialTransactionEntity)> GenerateTedFinancialTransactions(
        TedInTransferRequestDto request, 
        AccountEntity fromAccount, 
        AccountEntity toAccount)
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

        await financialTransactionRepository.InsertManyAsync(originFinancialTransaction, destinationFinancialTransaction);
        
        return (originFinancialTransaction, destinationFinancialTransaction);
    }
}