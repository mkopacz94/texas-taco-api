using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Core.Exceptions;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services.Verification;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth/verify")]
    public class VerificationController(
        IVerificationTokensRepository _verificationTokensRepository,
        IAuthenticationRepository _authRepository,
        IEmailVerificationService _emailVerificationService) : ControllerBase
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

        [HttpPost("resend")]
        public async Task<IActionResult> ResendVerificationEmail(
            [FromBody] AccountId accountId)
        {
            var account = await _authRepository.GetByIdAsync(accountId) 
                ?? throw new AccountDoesNotExistException(accountId);

            await _emailVerificationService.EnqueueVerificationEmail(account);

            return NoContent();
        }
    }
}
