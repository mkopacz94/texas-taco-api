using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrder(string id)
        {
            if (!Guid.TryParse(id, out var idGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given order ID ({id}) is not a valid GUID.");
            }

            return Ok();
        }
    }
}
