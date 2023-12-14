using BankChallenge.Business.Entities;
using BankChallenge.Business.Interfaces.Services;
using BankChallenge.Shared;
using BankChallenge.Shared.Dtos.Identity;
using BankChallenge.Shared.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BankChallenge.Business.Services;

public class TokenService : ITokenService
{
    public TokenInfoDto GenerateTokenInfo(AccountHolderEntity accountHolderEntity)
    {
        const int secondsToExpiresToken = 3600;
        var expirationDate = DateTime.UtcNow.AddSeconds(secondsToExpiresToken);

        var claims = new[]
        {
            new Claim("id", accountHolderEntity.Id.ToString() ?? string.Empty),
            new Claim("document", accountHolderEntity.Cpf ?? string.Empty),
        };

        var token = GenerateToken(Settings.AccessTokenSecret, claims, expirationDate);

        return new TokenInfoDto(token, secondsToExpiresToken);
    }

    private string GenerateToken(string secret, IEnumerable<Claim> claims, DateTime expirationDate)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = secret.ConvertToAscii();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationDate,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}