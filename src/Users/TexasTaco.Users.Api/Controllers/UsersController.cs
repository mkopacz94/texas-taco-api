using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;

namespace TexasTaco.Users.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        [AuthorizeRole(Role.Admin)]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok("Users");
        }
    }
}
