using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Api.Exceptions;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth/accounts")]
    public class AccountController(IHttpContextAccessor _httpContextAccessor) : ControllerBase
    {
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromQuery] string accountId)
        {
            string accountIdClaimType = "AccountId";

            string? userAccountId = _httpContextAccessor.HttpContext!.User.Claims
                .FirstOrDefault(c => c.Type == accountIdClaimType)
                ?.Value;

            return userAccountId is null 
                ? throw new ClaimNotExistException(accountIdClaimType) 
                : NoContent();
        }
    }
}
