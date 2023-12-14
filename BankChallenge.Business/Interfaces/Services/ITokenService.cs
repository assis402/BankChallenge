using BankChallenge.Business.Entities;
using BankChallenge.Shared.Dtos.Identity;

namespace BankChallenge.Business.Interfaces.Services;

public interface ITokenService
{
    public TokenInfoDto GenerateTokenInfo(AccountHolderEntity accountHolderEntity);
}