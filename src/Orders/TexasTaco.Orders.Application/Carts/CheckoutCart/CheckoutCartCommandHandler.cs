using MediatR;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Application.Carts.Exceptions;
using TexasTaco.Orders.Application.Carts.Mapping;

namespace TexasTaco.Orders.Application.Carts.CheckoutCart
{
    internal class CheckoutCartCommandHandler(
        ICartsRepository cartsRepository,
        ICheckoutCartsRepository checkoutCartsRepository)
        : IRequestHandler<CheckoutCartCommand, CheckoutCartDto>
    {
        private readonly ICartsRepository _cartRepository = cartsRepository;
        private readonly ICheckoutCartsRepository _checkoutCartsRepository
            = checkoutCartsRepository;

        public async Task<CheckoutCartDto> Handle(
            CheckoutCartCommand request,
            CancellationToken cancellationToken)
        {
            var cart = await _cartRepository
                .GetCartAsync(request.CartId)
                ?? throw new CartNotFoundException(request.CartId);

            var checkoutCart = cart.Checkout();

            if (cart.HasAssignedCheckoutCart())
            {
                cart.CheckoutCart!.UpdateCheckoutCart(cart);

                await _checkoutCartsRepository
                    .UpdateAsync(cart.CheckoutCart);

                return CheckoutCartMap.Map(cart.CheckoutCart);
            }

            await _checkoutCartsRepository
                .AddAsync(checkoutCart);

            return CheckoutCartMap.Map(checkoutCart);
        }
    }
}
