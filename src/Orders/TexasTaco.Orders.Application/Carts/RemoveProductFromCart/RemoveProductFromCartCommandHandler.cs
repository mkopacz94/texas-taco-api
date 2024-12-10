using MediatR;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Carts.Exceptions;

namespace TexasTaco.Orders.Application.Carts.RemoveProductFromCart
{
    internal sealed class RemoveProductFromCartCommandHandler(
        ICartsRepository cartsRepository,
        ILogger<RemoveProductFromCartCommandHandler> logger)
        : IRequestHandler<RemoveProductFromCartCommand>
    {
        private readonly ICartsRepository _cartsRepository = cartsRepository;
        private readonly ILogger<RemoveProductFromCartCommandHandler> _logger = logger;

        public async Task Handle(
            RemoveProductFromCartCommand request,
            CancellationToken cancellationToken)
        {
            var cart = await _cartsRepository
                .GetCartAsync(request.CartId)
                ?? throw new CartNotFoundException(request.CartId);

            cart.RemoveItem(request.CartProductId);

            await _cartsRepository.UpdateAsync(cart);

            _logger.LogInformation($"Successfully removed " +
                "product {cartProductId} from cart {cartId}.",
                request.CartProductId.Value,
                request.CartId.Value);
        }
    }
}
