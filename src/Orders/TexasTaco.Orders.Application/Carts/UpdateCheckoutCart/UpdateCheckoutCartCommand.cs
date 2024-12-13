using MediatR;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Application.Carts.UpdateCheckoutCart
{
    public sealed record UpdateCheckoutCartCommand(
        CheckoutCartId Id,
        PaymentType PaymentType,
        PickupLocation PickupLocation)
        : IRequest;
}
