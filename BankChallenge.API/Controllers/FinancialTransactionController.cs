using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Shared.Dtos.FinancialTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BankChallenge.API.Controllers;

[Authorize]
[ApiController]
[Route("v1/financialTransaction/")]
public class FinancialTransactionController(IFinancialTransactionService financialTransactionService) : BankChallengeControllerBase
{
    /// <summary>
    /// Obtém todas as transações associadas a um número de conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// GET /123456-0
    /// </code>
    ///
    /// Exemplo de resposta:
    /// <code>
    /// [
    ///    {
    ///        "Id": "5f68d25b3e7ef609f05e2d7b",
    ///        "AccountId": "123456-0",
    ///        "Amount": 50.00,
    ///        "Type": "Deposit",
    ///        "Status": "Completed",
    ///        "Category": "Income"
    ///    }
    /// ]
    /// </code>
    /// </remarks>
    /// <param name="accountNumber">Número da conta para buscar as transações.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpGet("{accountNumber}")]
    public async Task<ActionResult> GetAllTransactionsByAccountNumber([FromRoute] string accountNumber)
        => await financialTransactionService.GetAllTransactionsByAccountNumber(accountNumber, AccountHolderId);

    /// <summary>
    /// Obtém todas as dívidas bancárias associadas a um número de conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// GET /getAllDebtsByAccountNumber/123456-0
    /// </code>
    ///
    /// Exemplo de resposta:
    /// <code>
    /// [
    ///    {
    ///        "Id": "5f68d25b3e7ef609f05e2d7a",
    ///        "RequestedLoanAmount": 100.00,
    ///        "OriginalAmountToPay": 105.00,
    ///        "CurrentAmountToPay": 105.00,
    ///        "DueDate": "2023-12-14",
    ///        "Status": "Pending",
    ///        "Type": "Loan"
    ///    }
    /// ]
    /// </code>
    /// </remarks>
    /// <param name="accountNumber">Número da conta para buscar as dívidas bancárias.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpGet("getAllDebtsByAccountNumber/{accountNumber}")]
    public async Task<ActionResult> GetAllDebtsByAccountNumber([FromRoute] string accountNumber)
        => await financialTransactionService.GetAllDebtsByAccountNumber(accountNumber, AccountHolderId);

    /// <summary>
    /// Realiza um depósito em uma conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "accountNumber": "123456-0",
    ///    "amount": 1000.00
    /// }
    /// </code>
    ///
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "Operação realizada com sucesso.",
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
        => await financialTransactionService.Deposit(request, AccountHolderId);

    /// <summary>
    /// Realiza um saque de uma conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "accountNumber": "123456-0",
    ///    "amount": 500.00
    /// }
    /// </code>
    ///
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "Operação realizada com sucesso.",
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
        => await financialTransactionService.Withdraw(request, AccountHolderId);

    /// <summary>
    /// Realiza transferência bancária interna via TED.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "accountNumber": "123456-0",
    ///    "destinationAccountNumber": "123458-0",
    ///    "amount": 100.00
    /// }
    /// </code>
    ///
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "Operação realizada com sucesso.",
    ///    "statusCode": 200
    /// }
    /// </code>
    /// </remarks>
    /// <param name="request">DTO de requisição para transferir interna via TED.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("tedInTransfer")]
    public async Task<ActionResult> TedInTransfer(TedInTransferRequestDto request)
        => await financialTransactionService.TedInTransfer(request, AccountHolderId);

    /// <summary>
    /// Solicita um empréstimo.
    /// </summary>
    /// <remarks>
    /// OBS: Considerando que o projeto visa simplificar o processo bancário, o prazo para o pagamento do empréstimo é estabelecido para um mês no futuro, e a aplicação de juros é mantida fixa em 5%. <br/>
    ///
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "accountNumber": "123456-0",
    ///    "amount": 1000.00,
    ///    "paymentDate": "2023-12-31"
    /// }
    /// </code>
    ///
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "Operação realizada com sucesso.",
    ///    "statusCode": 200
    /// }
    /// </code>
    /// </remarks>
    /// <param name="request">DTO de requisição para solicitar um empréstimo.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("requestLoan")]
    public async Task<ActionResult> RequestLoan(LoanRequestDto request)
        => await financialTransactionService.RequestLoan(request, AccountHolderId);

    /// <summary>
    /// Quita uma dívida bancária (ex: Empréstimo).
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "debtId": "5f68d25b3e7ef609f05e2d7a",
    ///    "accountNumber": "123456-0",
    ///    "amount": 100.00
    /// }
    /// </code>
    ///
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "Pagamento total da dívida realizado com sucesso",
    ///    "statusCode": 200
    /// }
    /// </code>
    /// </remarks>
    /// <param name="request">DTO de requisição para quitar uma dívida bancária.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("payOffDebt")]
    public async Task<ActionResult> PayOffDebt(PayOffDebtRequestDto request)
        => await financialTransactionService.PayOffDebt(request, AccountHolderId);
}