using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public class CheckoutCartsController : ControllerBase
    {
        [HttpGet("{id}", Name = "GetCheckoutCart")]
        [HttpGet]
        public async Task<IActionResult> GetCheckoutCart(string id)
        {
            return Ok();
        }
    }
}
