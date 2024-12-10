using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Orders.Application.Carts.GetCart;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public sealed class CartController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetCart([FromQuery] string customerId)
        {
            var customerIdentifier = new CustomerId(Guid.Parse(customerId));
            var query = new GetCartQuery(customerIdentifier);
            var cart = await _mediator.Send(query);

            return Ok(cart);
        }
    }
}
