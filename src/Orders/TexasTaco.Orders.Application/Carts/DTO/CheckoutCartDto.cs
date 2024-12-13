using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Application.Carts.DTO
{
    public sealed record CheckoutCartDto(
        Guid Id,
        Guid CustomerId,
        List<CartProductDto> Products,
        PaymentType? PaymentType,
        PickupLocation? PickupLocation,
        decimal TotalPrice);
}
