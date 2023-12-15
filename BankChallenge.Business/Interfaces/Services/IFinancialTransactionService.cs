using BankChallenge.Business.Entities;
using BankChallenge.Shared.Dtos.FinancialTransaction;
using Matsoft.ApiResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BankChallenge.Business.Interfaces.Services;

public interface IFinancialTransactionService
{
    public Task<ApiResult> Withdraw(BaseTransactionRequestDto request, string accountHolderId);

    public Task<ApiResult> Deposit(BaseTransactionRequestDto request, string accountHolderId);

    public Task Deposit(decimal amount, AccountEntity accountEntity, IClientSessionHandle session);

    public Task<ApiResult> TedInTransfer(TedInTransferRequestDto request, string accountHolderId);

    public Task<ApiResult> PayOffDebt(PayOffDebtRequestDto request, string accountHolderId);

    public Task<ActionResult> RequestLoan(LoanRequestDto request, string accountHolderId);

    public Task<ApiResult> GetAllDebtsByAccountNumber(string accountNumber, string accountHolderId);

    public Task<ApiResult> GetAllTransactionsByAccountNumber(string accountNumber, string accountHolderId);
}