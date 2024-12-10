using MediatR;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Carts.Exceptions;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Cart.Exceptions;

namespace TexasTaco.Orders.Application.Carts.UpdateProductQuantity
{
    internal sealed class UpdateProductQuantityCommandHandler(
        ICartsRepository cartsRepository,
        ILogger<UpdateProductQuantityCommandHandler> logger)
        : IRequestHandler<UpdateProductQuantityCommand, CartProduct>
    {
        private readonly ICartsRepository _cartsRepository = cartsRepository;
        private readonly ILogger<UpdateProductQuantityCommandHandler> _logger = logger;

        public async Task<CartProduct> Handle(
            UpdateProductQuantityCommand request,
            CancellationToken cancellationToken)
        {
            var cart = await _cartsRepository
                .GetCartAsync(request.CartId)
                ?? throw new CartNotFoundException(request.CartId);

            var product = cart
                .Products
                .SingleOrDefault(p => p.Id == request.ProductId)
                ?? throw new CartProductNotFoundException(request.ProductId);

            var oldQuantity = product.Quantity;

            product.ChangeQuantity(request.Quantity);

            await _cartsRepository.UpdateAsync(cart);

            _logger.LogInformation($"Successfully changed " +
                "product {cartProductId} quantity in cart {cartId} " +
                "from {oldQuantity} to {newQuantity}.",
                request.ProductId.Value,
                request.CartId.Value,
                oldQuantity,
                request.Quantity);

            return product;
        }
    }
}
