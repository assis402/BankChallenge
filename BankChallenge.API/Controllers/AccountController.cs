using System.Net;
using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Shared.Dtos.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankChallenge.API.Controllers;

[Authorize]
[ApiController]
[Route("v1/account/")]
public class AccountController(IAccountService accountService) : BankChallengeControllerBase
{
    /// <summary>
    /// Realiza um depósito em uma conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "AccountNumber": "123456-0",
    ///    "Amount": 1000.00
    /// }
    /// </code>
    /// 
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "Transação realizada com sucesso.",
    ///    "statusCode": 200
    /// }
    /// </code>
    /// </remarks>
    /// <param name="request">DTO de requisição para depósito.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("deposit")]
    public async Task<ActionResult> Deposit(BaseTransactionRequestDto request)
        => await accountService.Deposit(request, AccountHolderId);
    
    /// <summary>
    /// Realiza um saque de uma conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "AccountNumber": "123456-0",
    ///    "Amount": 500.00
    /// }
    /// </code>
    /// 
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "Transação realizada com sucesso.",
    ///    "statusCode": 200
    /// }
    /// </code>
    /// </remarks>
    /// <param name="request">DTO de requisição para saque.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("withdraw")]
    public async Task<ActionResult> Withdraw(BaseTransactionRequestDto request)
        => await accountService.Withdraw(request, AccountHolderId);
    
    /// <summary>
    /// Realiza transferência bancária interna via TED.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    ///     {
    ///        "accountNumber": "123456-0", 
    ///        "destinationAccountNumber": "123458-0", 
    ///        "amount": 100.00
    ///     }
    /// </code>
    /// 
    /// Exemplo de resposta:
    /// <code>
    ///     {
    ///        "success": true,
    ///        "message": "Transferência realizada com sucesso.",
    ///        "statusCode": 200
    ///     }
    /// </code>
    /// </remarks>
    /// <param name="request">DTO de requisição para transferir interna via TED.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("tedInTransfer")]
    public async Task<ActionResult> TedInTransfer(TedInTransferRequestDto request)
        => await accountService.TedInTransfer(request, AccountHolderId);
    
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("requestLoan")]
    public async Task<ActionResult> RequestLoan(LoanRequestDto request)
        => await accountService.RequestLoan(request, AccountHolderId);
    
    /// <summary>
    /// Quita uma dívida bancária.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    ///     {
    ///        "debtId": "5f68d25b3e7ef609f05e2d7a", 
    ///        "accountNumber": "123456-0", 
    ///        "amount": 100.00
    ///     }
    /// </code>
    /// 
    /// Exemplo de resposta:
    /// <code>
    ///     {
    ///        "success": true,
    ///        "message": "Pagamento total da dívida realizado com sucesso",
    ///        "statusCode": 200
    ///     }
    /// </code>
    /// </remarks>
    /// <param name="request">DTO de requisição para quitar uma dívida bancária.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("fulfillDebt")]
    public async Task<ActionResult> FulfillDebt(FulfillDebtRequestDto request)
        => await accountService.FulfillDebt(request, AccountHolderId);
}