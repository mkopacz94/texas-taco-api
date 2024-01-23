using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Core.Exceptions;
using TexasTaco.Authentication.Core.Repositories;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth/verify")]
    public class VerificationController(
        IVerificationTokensRepository _verificationTokensRepository,
        IAuthenticationRepository _authRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> VerifyAccount([FromQuery] string token)
        {
            var verificationToken = await _verificationTokensRepository
                .GetByTokenValueAsync(Guid.Parse(token));

            if(verificationToken == null)
            {
                return BadRequest();
            }

            if(verificationToken.IsExpired)
            {
                throw new VerificationTokenExpiredException();
            }

            var accountToBeVerified = await _authRepository
                .GetByIdAsync(verificationToken.AccountId) 
                ?? throw new AccountAssociatedWithTokenNotFoundException(verificationToken);

            accountToBeVerified.MarkAsVerified();
            await _authRepository.UpdateAccountAsync(accountToBeVerified);

            return Ok();
        }
    }
}
