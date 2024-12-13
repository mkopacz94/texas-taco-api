using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Application.Carts.GetCheckoutCart;
using TexasTaco.Orders.Application.Carts.PlaceOrder;
using TexasTaco.Orders.Application.Carts.UpdateCheckoutCart;
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCheckoutCart(
            string id,
            [FromBody] UpdateCheckoutCartDto updateDto)
        {
            if (!Guid.TryParse(id, out var idGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given checkout cart ID ({id}) is not a valid GUID.");
            }

            var command = new UpdateCheckoutCartCommand(
                new CheckoutCartId(idGuid),
                updateDto.PaymentType,
                updateDto.PickupLocation);

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPost("{id}/place-order")]
        public async Task<IActionResult> PlaceOrder(
            string id)
        {
            if (!Guid.TryParse(id, out var idGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given checkout cart ID ({id}) is not a valid GUID.");
            }

            var command = new PlaceOrderCommand(new CheckoutCartId(idGuid));

            var orderDto = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(OrdersController.GetOrder),
                "orders",
                new { id = orderDto.Id.ToString() },
                orderDto);
        }
    }
}
