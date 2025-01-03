﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Api.Configuration;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core.Data;
using TexasTaco.Authentication.Core.DTO;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Authentication.Core.Services.Verification;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;
using TexasTaco.Shared.Services;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Api.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/auth")]
    [ApiController]
    public class AuthenticationController(
        IUnitOfWork _unitOfWork,
        IAuthenticationRepository _authRepo,
        IOutboxMessagesRepository<OutboxMessage<AccountDeletedEventMessage>>
            _accountDeletedOutboxMessagesRepository,
        IEmailVerificationService _emailVerificationService,
        ICookieService _cookieService,
        ISessionStorage _sessionStorage,
        IClaimsService _claimsManager,
        SessionConfiguration _sessionConfiguration) : ControllerBase
    {
        [MapToApiVersion(1)]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto signUpData)
        {
            var emailAddress = new EmailAddress(signUpData.Email);
            var account = await _authRepo.CreateAccountAsync(emailAddress, Role.Customer, signUpData.Password);
            await _emailVerificationService.EnqueueVerificationEmail(account);

            return NoContent();
        }

        [MapToApiVersion(1)]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDto signInData)
        {
            var emailAddress = new EmailAddress(signInData.Email);
            var account = await _authRepo.AuthenticateAccountAsync(emailAddress, signInData.Password);

            var sessionExpirationDate = DateTime.UtcNow
                .AddMinutes(_sessionConfiguration.ExpirationMinutes);

            var sessionId = await _sessionStorage.CreateSession(account.Id, sessionExpirationDate);

            await _claimsManager.SetAccountClaims(account);
            SetSessionCookies(account.Id, sessionId, sessionExpirationDate);

            var signInResult = new SignInResultDto(sessionId, account.Id);

            return Ok(signInResult);
        }

        [MapToApiVersion(1)]
        [HttpGet("session-valid")]
        public async Task<IActionResult> IsSessionValid([FromQuery] string accountId, [FromQuery] string sessionId)
        {
            var accountIdentifier = new AccountId(Guid.Parse(accountId));
            var sessionIdentifier = new SessionId(Guid.Parse(sessionId));
            var session = await _sessionStorage.GetSession(accountIdentifier, sessionIdentifier);

            if (session is null || !session.IsValid())
            {
                return Unauthorized();
            }

            if (session.IsBeforeHalfOfExpirationTime())
            {
                return Ok(session);
            }

            var expirationTimespan = TimeSpan
                .FromMinutes(_sessionConfiguration.ExpirationMinutes);

            session.ExtendSession(expirationTimespan);

            await _sessionStorage.UpdateSession(accountIdentifier, session);

            return Ok(session);
        }

        [MapToApiVersion(1)]
        [AuthorizeRole(Role.Admin)]
        [HttpPut("revoke-session")]
        public async Task<IActionResult> RevokeSession([FromQuery] string accountId, [FromQuery] string sessionId)
        {
            var accountIdentifier = new AccountId(Guid.Parse(accountId));
            var sessionIdentifier = new SessionId(Guid.Parse(sessionId));
            var session = await _sessionStorage.GetSession(accountIdentifier, sessionIdentifier);

            if (session is null || !session.IsValid())
            {
                return NoContent();
            }

            session.Revoke();
            await _sessionStorage.UpdateSession(accountIdentifier, session);

            return NoContent();
        }

        [MapToApiVersion(1)]
        [AuthorizeRole(Role.Admin)]
        [HttpDelete("delete-account/{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var accountIdGuid = Guid.Parse(id);
            var accountId = new AccountId(accountIdGuid);

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            await _authRepo.DeleteAsync(accountId);

            var accountDeletedEventMessage = new AccountDeletedEventMessage(
                Guid.NewGuid(),
                accountIdGuid);

            var outboxMessage = new OutboxMessage<AccountDeletedEventMessage>(
                accountDeletedEventMessage);

            await _accountDeletedOutboxMessagesRepository
                .AddAsync(outboxMessage);

            await transaction.CommitAsync();

            return NoContent();
        }

        private void SetSessionCookies(AccountId accountId, SessionId sessionId, DateTime expirationDate)
        {
            var sessionCookieOptions = new CookieOptions
            {
                Expires = new DateTimeOffset(expirationDate),
                HttpOnly = true,
                Secure = true
            };

            _cookieService.SetCookie(
                CookiesNames.AccountId,
                accountId.Value.ToString(),
                sessionCookieOptions);

            _cookieService.SetCookie(
                CookiesNames.SessionId,
                sessionId.Value.ToString(),
                sessionCookieOptions);
        }
    }
}
