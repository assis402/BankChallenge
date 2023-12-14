using BankChallenge.Business.Interfaces.Repositories;
using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Business.Services;
using BankChallenge.Infrasctructure;
using BankChallenge.Infrasctructure.Repositories;

namespace BankChallenge.API.Configurations;

public static class ServicesConfiguration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<BankChallengeContextDb>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountHolderService, AccountHolderService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountHolderRepository, AccountHolderRepository>();
        services.AddScoped<IFinancialTransactionRepository, FinancialTransactionRepository>();
        services.AddScoped<IDebtRepository, DebtRepository>();
    }
}