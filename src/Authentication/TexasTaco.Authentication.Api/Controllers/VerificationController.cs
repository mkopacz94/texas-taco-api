using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Core.Data;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.Exceptions;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services.Verification;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Api.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/auth/verify")]
    [ApiController]
    public class VerificationController(
        IUnitOfWork _unitOfWork,
        IVerificationTokensRepository _verificationTokensRepository,
        IAuthenticationRepository _authRepository,
        IAccountCreatedOutboxRepository _accountCreatedOutboxRepository,
        IEmailVerificationService _emailVerificationService) : ControllerBase
    {
        [MapToApiVersion(1)]
        [HttpPost]
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

            var transaction = await _unitOfWork.BeginTransactionAsync();

            await _authRepository.UpdateAccount(accountToBeVerified);

            var accountCreatedOutboxMessage = new AccountCreatedOutbox(
                accountToBeVerified.Id,
                accountToBeVerified.Email);

            await _accountCreatedOutboxRepository.AddAsync(accountCreatedOutboxMessage);

            await transaction.CommitAsync();

            return Ok();
        }

        [MapToApiVersion(1)]
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
