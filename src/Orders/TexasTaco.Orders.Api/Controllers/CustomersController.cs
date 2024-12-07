using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public sealed class CustomersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetCustomer([FromQuery] string customerId)
        {
            return await Task.FromResult(Ok("Test"));
        }
    }
}
