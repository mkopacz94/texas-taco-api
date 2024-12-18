using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Orders.Application.Carts.CheckoutCart;
using TexasTaco.Orders.Application.Carts.GetCart;
using TexasTaco.Orders.Application.Carts.RemoveProductFromCart;
using TexasTaco.Orders.Application.Carts.UpdateProductQuantity;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/orders/[controller]")]
    [Authorize]
    public sealed class CartsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetCart([FromQuery] string customerId)
        {
            var customerIdentifier = new CustomerId(Guid.Parse(customerId));
            var query = new GetCartQuery(customerIdentifier);
            var cartDto = await _mediator.Send(query);

            return Ok(cartDto);
        }

        [HttpPut("{cartId}/products/{cartProductId}")]
        public async Task<IActionResult> UpdateProductQuantity(
            string cartId,
            string cartProductId,
            [FromQuery] int quantity)
        {
            if (!Guid.TryParse(cartId, out var cartIdGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given cart ID ({cartId}) is not a valid GUID.");
            }

            if (!Guid.TryParse(cartProductId, out var cartProductIdGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given cart product ID ({cartProductId}) is not a valid GUID.");
            }

            var command = new UpdateProductQuantityCommand(
                new CartId(cartIdGuid),
                new CartProductId(cartProductIdGuid),
                quantity);

            var productDto = await _mediator.Send(command);

            return Ok(productDto);
        }

        [HttpDelete("{cartId}/products/{cartProductId}")]
        public async Task<IActionResult> RemoveProductFromCart(
            string cartId,
            string cartProductId)
        {
            if (!Guid.TryParse(cartId, out var cartIdGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given cart ID ({cartId}) is not a valid GUID.");
            }

            if (!Guid.TryParse(cartProductId, out var cartProductIdGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given cart product ID ({cartProductId}) is not a valid GUID.");
            }

            var command = new RemoveProductFromCartCommand(
                new CartId(cartIdGuid),
                new CartProductId(cartProductIdGuid));

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPost("{cartId}/checkout")]
        public async Task<IActionResult> CheckoutCart(string cartId)
        {
            if (!Guid.TryParse(cartId, out var cartIdGuid))
            {
                throw new InvalidRequestParametersException(
                    $"Given cart ID ({cartId}) is not a valid GUID.");
            }

            var command = new CheckoutCartCommand(
                new CartId(cartIdGuid));

            var checkoutCartDto = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(CheckoutController.GetCheckoutCart),
                "checkout",
                new { id = checkoutCartDto.Id.ToString() },
                checkoutCartDto);
        }
    }
}
