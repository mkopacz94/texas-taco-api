using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Core.Dto;
using TexasTaco.Authentication.Core.Exceptions;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController(IAuthenticationRepository _authRepo) : ControllerBase
    {
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpData signUpData)
        {
            try
            {
                var emailAddress = new EmailAddress(signUpData.Email.ToLower());

                if (await _authRepo.EmailAlreadyExists(emailAddress))
                {
                    return BadRequest("Email has already been registered.");
                }

                await _authRepo.CreateAccount(emailAddress, signUpData.Password);

                return Created();
            }
            catch(InvalidEmailAddressFormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
