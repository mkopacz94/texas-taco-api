using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Api.Configuration;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core.DTO;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Authentication.Core.Services.Verification;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;
using TexasTaco.Shared.Services;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController(
        IAuthenticationRepository _authRepo,
        IEmailVerificationService _emailVerificationService,
        ICookieService _cookieService,
        ISessionStorage _sessionStorage,
        IClaimsService _claimsManager,
        SessionConfiguration _sessionConfiguration) : ControllerBase
    {
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto signUpData)
        {
            var emailAddress = new EmailAddress(signUpData.Email);
            var account = await _authRepo.CreateAccountAsync(emailAddress, Role.Customer, signUpData.Password);
            await _emailVerificationService.EnqueueVerificationEmail(account);

            return NoContent();
        }

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

        [HttpGet("session-valid")]
        public async Task<IActionResult> IsSessionValid([FromQuery] string accountId, [FromQuery] string sessionId)
        {
            var accountIdentifier = new AccountId(Guid.Parse(accountId));
            var sessionIdentifier = new SessionId(Guid.Parse(sessionId));
            var session = await _sessionStorage.GetSession(accountIdentifier, sessionIdentifier);

            if(session is null || !session.IsValid())
            {
                return Unauthorized();
            }

            if(session.IsBeforeHalfOfExpirationTime())
            {
                return Ok(session);
            }

            var expirationTimespan = TimeSpan
                .FromMinutes(_sessionConfiguration.ExpirationMinutes);

            session.ExtendSession(expirationTimespan);

            await _sessionStorage.UpdateSession(accountIdentifier, session);

            return Ok(session);
        }

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
