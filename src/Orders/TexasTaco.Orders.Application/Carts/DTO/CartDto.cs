namespace TexasTaco.Orders.Application.Carts.DTO
{
    public sealed record CartDto(
        Guid Id,
        Guid CustomerId,
        List<CartProductDto> Products,
        decimal TotalPrice);
}
