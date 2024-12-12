using TexasTaco.Orders.Application.Shared.DTO;

namespace TexasTaco.Orders.Application.Carts.DTO
{
    public sealed record CheckoutCartDto(
        Guid Id,
        Guid CustomerId,
        DeliveryAddressDto? DeliveryAddress,
        List<CartProductDto> Products);
}
