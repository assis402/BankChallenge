using BankChallenge.Business.Entities;
using BankChallenge.Shared.Dtos.Account;
using Matsoft.ApiResults;
using Microsoft.AspNetCore.Mvc;

namespace BankChallenge.Business.Interfaces.Services;

public interface IAccountService
{
    public Task<AccountEntity> CreateCheckingAccount(string accountHolderId);

    public Task<ApiResult> GetAllAccountsByAccountHolderId(string accountHolderId);

    public Task<ApiResult> Withdraw(BaseTransactionRequestDto request, string accountHolderId);

    public Task<ApiResult> Deposit(BaseTransactionRequestDto request, string accountHolderId);

    public Task Deposit(decimal amount, AccountEntity accountEntity);

    public Task<ApiResult> TedInTransfer(TedInTransferRequestDto request, string accountHolderId);

    public Task<ApiResult> PayOffDebt(PayOffDebtRequestDto request, string accountHolderId);

    public Task<ActionResult> RequestLoan(LoanRequestDto request, string accountHolderId);
}