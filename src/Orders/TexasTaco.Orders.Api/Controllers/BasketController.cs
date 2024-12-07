using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Orders.Application.Baskets.GetBasket;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public class BasketController(
        IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasket([FromQuery] string customerId)
        {
            var customerIdentifier = new CustomerId(Guid.Parse(customerId));
            var query = new GetBasketQuery(customerIdentifier);
            var basket = await _mediator.Send(query);

            return Ok(basket);
        }
    }
}
