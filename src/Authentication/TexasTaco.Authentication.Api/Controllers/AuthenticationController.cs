using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Core.DTO;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController(IAuthenticationRepository _authRepo) : ControllerBase
    {
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
            var sessionId = await _authRepo.AuthenticateAccount(emailAddress, signInData.Password);

            return Ok(sessionId);
        }
    }
}
