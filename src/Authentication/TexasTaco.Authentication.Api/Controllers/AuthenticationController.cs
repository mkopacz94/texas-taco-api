using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core.DTO;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Authentication.Core.Services.Notifications;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController(
        IAuthenticationRepository _authRepo,
        ISessionStorage _sessionStorage,
        ICookieService _cookieService,
        IClaimsService _claimsManager,
        IEmailSendingService _emailService) : ControllerBase
    {
        private const string SessionIdCookieName = "session-id";

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto signUpData)
        {
            var emailAddress = new EmailAddress(signUpData.Email);
            await _authRepo.CreateAccount(emailAddress, Role.Customer, signUpData.Password);

            return NoContent();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDto signInData)
        {
            _emailService.Send(new Core.Models.EmailNotification("Test", "Test", new EmailAddress("m.kopacz94@gmail.com"), new EmailAddress("m.kopacz94@gmail.com")));

            var emailAddress = new EmailAddress(signInData.Email);
            var account = await _authRepo.AuthenticateAccount(emailAddress, signInData.Password);

            var sessionExpirationDate = DateTime.UtcNow.AddMinutes(1);
            var sessionId = await _sessionStorage.CreateSession(sessionExpirationDate);

            Console.WriteLine(account.Role.ToString());
            await _claimsManager.SetAccountClaims(account);
            SetSessionCookie(sessionId, sessionExpirationDate);

            return Ok(sessionId);
        }

        [HttpGet("session-valid")]
        public async Task<IActionResult> IsSessionValid([FromQuery] string sessionId)
        {
            var sessionIdObject = new SessionId(Guid.Parse(sessionId));
            var session = await _sessionStorage.GetSession(sessionIdObject);

            if(session is null || session.ExpirationDate <  DateTime.UtcNow)
            {
                return Unauthorized();
            }

            session.ExtendSession(TimeSpan.FromMinutes(1));
            await _sessionStorage.UpdateSession(sessionIdObject, session);

            SetSessionCookie(sessionIdObject, session.ExpirationDate);

            return Ok();
        }

        [AuthorizeRole(Role.Admin)]
        [HttpPost("revoke-session")]
        public IActionResult RevokeSession([FromQuery] string sessionId)
        {
            return Ok(sessionId);
        }

        private void SetSessionCookie(SessionId sessionId, DateTime expirationDate)
        {
            _cookieService.SetCookie(SessionIdCookieName, sessionId.Value.ToString(),
                new CookieOptions
                {
                    Expires = new DateTimeOffset(expirationDate),
                    HttpOnly = true,
                    Secure = true
                });
        }
    }
}
