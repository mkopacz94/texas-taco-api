namespace TexasTaco.Orders.Application.Carts.DTO
{
    public sealed record CartProductDto(
        Guid Id,
        Guid ProductId,
        string Name,
        decimal Price,
        string? PictureUrl,
        int Quantity);
}
