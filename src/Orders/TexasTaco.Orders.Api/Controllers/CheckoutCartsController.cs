using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Orders.Application.Carts.GetCheckoutCart;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public class CheckoutCartsController(
        IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}", Name = "GetCheckoutCart")]
        [HttpGet]
        public async Task<IActionResult> GetCheckoutCart(string id)
        {
            if (!Guid.TryParse(id, out var idGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given checkout cart ID ({id}) is not a valid GUID.");
            }

            var query = new GetCheckoutCartQuery(
                new CheckoutCartId(idGuid));

            var checkoutCartDto = await _mediator.Send(query);

            return Ok(checkoutCartDto);
        }
    }
}
