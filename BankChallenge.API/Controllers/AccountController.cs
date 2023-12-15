using BankChallenge.Business.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BankChallenge.API.Controllers;

[Authorize]
[ApiController]
[Route("v1/account/")]
public class AccountController(IAccountService accountService) : BankChallengeControllerBase
{
    /// <summary>
    /// Obtém todas as contas associadas ao titular da conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de resposta:
    /// <code>
    /// [
    ///     {
    ///         "accountNumber": "123456-0",
    ///         "balance": 1500.00,
    ///         "type": "Savings",
    ///         "status": "Active",
    ///         "openingDate": "2023-01-01"
    ///     },
    ///     {
    ///         "accountNumber": "789012-1",
    ///         "balance": 500.00,
    ///         "type": "Checking",
    ///         "status": "Inactive",
    ///         "openingDate": "2022-05-15"
    ///     }
    /// ]
    /// </code>
    /// </remarks>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpGet]
    public async Task<ActionResult> GetAllAccountsByAccountHolderId()
        => await accountService.GetAllAccountsByAccountHolderId(AccountHolderId);
}