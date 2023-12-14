using BankChallenge.Shared.Dtos.Identity;
using Matsoft.ApiResults;

namespace BankChallenge.Business.Interfaces.Services;

public interface IAccountHolderService
{
    public Task<ApiResult> SignIn(SignInDto signInDto);

    public Task<ApiResult> SignUp(SignUpDto request);
}