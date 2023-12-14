using System.Net;
using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Shared.Dtos.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankChallenge.API.Controllers;

[ApiController]
[Route("v1/identity/")]
public class IdentityController(IAccountHolderService accountHolderService) : BankChallengeControllerBase
{
    /// <summary>
    /// Realiza o login do titular da conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "cpf": "12345678901",
    ///    "password": "senha123@"
    /// }
    /// </code>
    /// 
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "SignIn realizado com sucesso.",
    ///    "statusCode": 200
    /// }
    /// </code>
    /// </remarks>
    /// <param name="signInDto">DTO de requisição para login.</param>
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [HttpPost("signIn")]
    public async Task<ActionResult> SignIn(SignInDto signInDto) => await accountHolderService.SignIn(signInDto);
    
    /// <summary>
    /// Realiza o registro de um novo titular da conta.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    /// <code>
    /// {
    ///    "email": "joao@email.com",
    ///    "password": "senha123@",
    ///    "cpf": "12345678901",
    ///    "name": "João Silva",
    ///    "birthdate": "1990-01-01",
    ///    "address": "Rua A, 123",
    ///    "initialDeposit": 1000.00
    /// }
    /// </code>
    /// 
    /// Exemplo de resposta:
    /// <code>
    /// {
    ///    "success": true,
    ///    "message": "Cadastro no banco realizado e conta corrente criada com sucesso.",
    ///    "data": {
    ///         "AccountNumber": "123456-0",
    ///         "Balance": 5000.00,
    ///         "Type": "CheckingAccount",
    ///         "Status": "Active",
    ///         "OpeningDate": "2022-01-15"
    ///     }
    ///    "statusCode": 200
    /// }
    /// </code>
    /// </remarks>
    /// <param name="signUpDto">DTO de requisição para cadastro no banco.</param>
    [HttpPost("signUp")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> SignUp(SignUpDto signUpDto) => await accountHolderService.SignUp(signUpDto);
}