using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Application.Carts.DTO
{
    public sealed record UpdateCheckoutCartDto(
        PaymentType PaymentType,
        PickupLocation PickupLocation);
}
