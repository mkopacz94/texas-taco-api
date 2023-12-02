using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core.DTO;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController(
        IAuthenticationRepository _authRepo,
        ISessionStorage _sessionStorage,
        ICookieService _cookieService,
        IClaimsService _claimsManager) : ControllerBase
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
            var emailAddress = new EmailAddress(signInData.Email);
            var account = await _authRepo.AuthenticateAccount(emailAddress, signInData.Password);

            var sessionExpirationDate = DateTime.UtcNow.AddMinutes(1);
            var sessionId = await _sessionStorage.CreateSession(sessionExpirationDate);

            await _claimsManager.SetAccountClaims(account);

            _cookieService.SetCookie(SessionIdCookieName, sessionId.Value.ToString(),
                new CookieOptions
                {
                    Expires = new DateTimeOffset(sessionExpirationDate),
                    HttpOnly = true,
                    Secure = true
                });

            return Ok(sessionId);
        }
    }
}
