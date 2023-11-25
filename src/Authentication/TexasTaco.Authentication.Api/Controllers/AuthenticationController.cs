using Microsoft.AspNetCore.Mvc;
using TexasTaco.Authentication.Core.Repositories;

namespace TexasTaco.Authentication.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authRepo;

        public AuthenticationController(IAuthenticationRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            _authRepo.TestMethod();
            return Ok("Hello world!");
        }
    }
}
