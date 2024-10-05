using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public class BasketController : ControllerBase
    {
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetBasket(string accountId)
        {
            return await Task.FromResult(Ok(accountId));
        }
    }
}
