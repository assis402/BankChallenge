using Matsoft.ApiResults.CustomAttributes;
using System.ComponentModel;
using System.Net;

namespace BankChallenge.Business.Enums.Messages;

public enum BankChallengeMessage
{
    #region Identity

    [StatusCode(HttpStatusCode.OK)]
    [Description("SignIn realizado com sucesso.")]
    SignIn_Success,

    [StatusCode(HttpStatusCode.Created)]
    [Description("Cadastro no banco realizado e conta corrente criada com sucesso.")]
    SignUp_Success,

    #endregion Identity

    #region Debt

    [StatusCode(HttpStatusCode.OK)]
    [Description("Pagamento parcial da dívida realizado com sucesso.")]
    PartiallyPayDebt_Success,

    [StatusCode(HttpStatusCode.OK)]
    [Description("Pagamento total da dívida realizado com sucesso.")]
    PayOffDebt_Success,

    #endregion Debt

    #region Transaction

    [StatusCode(HttpStatusCode.OK)]
    [Description("Operação realizada com sucesso.")]
    Transaction_Success

    #endregion Transaction
}