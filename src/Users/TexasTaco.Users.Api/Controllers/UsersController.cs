using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TexasTaco.Users.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok("Users");
        }
    }
}
