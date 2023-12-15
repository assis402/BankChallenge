using Matsoft.ApiResults.CustomAttributes;
using System.ComponentModel;
using System.Net;

namespace BankChallenge.Business.Enums.Messages;

public enum BankChallengeError
{
    #region Application

    [Description("A requisição está inválida.")]
    Application_Error_InvalidRequest,

    [StatusCode(HttpStatusCode.InternalServerError)]
    [Description("Ocorreu um erro interno na aplicação, por favor, tente novamente.")]
    Application_Error_General,

    [StatusCode(HttpStatusCode.NotFound)]
    [Description("Registro(s) não encontrado(s).")]
    Application_Error_NotFound,

    #endregion Application

    #region Identity

    [Description("Os dados fornecidos estão incorretos.")]
    SignIn_Error_WrongCpfOrPassword,

    [Description("CPF informado é inválido.")]
    SignUp_Validation_InvalidCpf,

    [Description("Já existe o cadastro de um titular com os dados informados. Cada conta possui CPF e E-mail únicos.")]
    SignUp_Error_AccountHolderAlreadyExists,

    [Description("Senha informada não preenche os requisitos solicitados: Pelo menos uma letra maiúscula, uma letra minúscula, um dígito e um caractere especial.")]
    SignUp_Validation_InvalidPasswordRules,

    [Description("'Email' está inválido.")]
    SignUp_Validation_InvalidEmail,

    [Description("O titular da conta deve ter mais de 18 anos.")]
    SignUp_Validation_InvalidAge,

    [Description("A valor máximo atual do depósito inicial é de R$ 100.000,00.")]
    SignUp_Validation_InvalidInitialDeposit,

    #endregion Identity

    #region AccountHolder

    [Description("Este cadastro está inativo.")]
    AccountHolder_Validation_Inactive,

    #endregion AccountHolder

    #region Account

    [StatusCode(HttpStatusCode.Forbidden)]
    [Description("Operação não permitida para esta conta.")]
    Account_Error_ForbiddenOperation,

    [StatusCode(HttpStatusCode.NotFound)]
    [Description("A conta de origem informada não foi encontrada.")]
    Account_Origin_Error_NonExists,

    [Description("A conta de origem está inativa.")]
    Account_Origin_Validation_Inactive,

    [StatusCode(HttpStatusCode.NotFound)]
    [Description("A conta de destino informada não foi encontrada.")]
    Account_Destination_Error_NonExists,

    [Description("A conta de destino está inativa.")]
    Account_Destination_Validation_Inactive,

    [Description("O seu saldo é insuficiente para esta transação.")]
    Account_Validation_InsufficientBalance,

    [Description("É necessário informar o Id da conta.")]
    Account_Validation_AccountIdIsRequired,

    [StatusCode(HttpStatusCode.NotFound)]
    [Description("O débito bancário informado não foi encontrado.")]
    Debt_Error_NonExists,

    [Description("O débito bancário informado já está quitado.")]
    Debt_Validation_IsSettled,

    [Description("O débito bancário está com o prazo de pagamento vencido. Realize uma nova negociação junto ao banco para quitar sua dívida.")]
    Debt_Validation_IsOverdue,

    [StatusCode(HttpStatusCode.Forbidden)]
    [Description("O débito bancário informado não pertence ao titular desta conta.")]
    Debt_Error_InvalidAccountHolder,

    [Description("O valor para realizar a operação precisa ser maior que zero.")]
    Transaction_Validation_InsufficientAmount,

    // Foi estabelecido um limite máximo para empréstimos com o propósito de simplificar a realização de testes
    [Description("A valor máximo atual de empréstimos é de R$ 20.000,00.")]
    RequestLoan_Validation_InvalidAmount,

    // Foi estabelecido um limite de um empréstimo por conta com o propósito de simplificar a realização de testes
    [Description("Não é permitido solicitar um novo empréstimo enquanto o empréstimo ativo não for quitado.")]
    RequestLoan_Validation_Limit

    #endregion Account
}