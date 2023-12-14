using Microsoft.AspNetCore.Mvc;

namespace BankChallenge.API.Controllers;

public class BankChallengeControllerBase : ControllerBase
{
    protected string AccountHolderId => User.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value;
}