using MediatR;
using TexasTaco.Orders.Application.Carts.Exceptions;

namespace TexasTaco.Orders.Application.Carts.CheckoutCart
{
    internal class CheckoutCartCommandHandler(
        ICartsRepository cartsRepository,
        ICheckoutCartsRepository checkoutCartsRepository)
        : IRequestHandler<CheckoutCartCommand, Domain.Cart.CheckoutCart>
    {
        private readonly ICartsRepository _cartRepository = cartsRepository;
        private readonly ICheckoutCartsRepository _checkoutCartsRepository
            = checkoutCartsRepository;

        public async Task<Domain.Cart.CheckoutCart> Handle(
            CheckoutCartCommand request,
            CancellationToken cancellationToken)
        {
            var cart = await _cartRepository
                .GetCartAsync(request.CartId)
                ?? throw new CartNotFoundException(request.CartId);

            var checkoutCart = cart.Checkout();

            if (cart.CheckoutCart is not null)
            {
                cart.CheckoutCart.UpdateCheckoutCart(cart);
                await _checkoutCartsRepository.UpdateAsync(cart.CheckoutCart);
                return cart.CheckoutCart;
            }

            await _checkoutCartsRepository
                .AddAsync(checkoutCart);

            return checkoutCart;
        }
    }
}
