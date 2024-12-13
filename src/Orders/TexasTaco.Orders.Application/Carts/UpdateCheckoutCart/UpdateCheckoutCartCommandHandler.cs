using MediatR;
using TexasTaco.Orders.Application.Carts.Exceptions;

namespace TexasTaco.Orders.Application.Carts.UpdateCheckoutCart
{
    internal class UpdateCheckoutCartCommandHandler(
        ICheckoutCartsRepository _checkoutCartsRepository)
        : IRequestHandler<UpdateCheckoutCartCommand>
    {
        public async Task Handle(UpdateCheckoutCartCommand request, CancellationToken cancellationToken)
        {
            var checkoutCart = await _checkoutCartsRepository
                .GetAsync(request.Id)
                ?? throw new CheckoutCartNotFoundException(request.Id);

            checkoutCart.SetPaymentType(request.PaymentType);
            checkoutCart.SetPickupLocation(request.PickupLocation);

            await _checkoutCartsRepository.UpdateAsync(checkoutCart);
        }
    }
}
