namespace TexasTaco.Orders.Application.Orders.DTO
{
    public sealed record OrderLineDto(
        Guid Id,
        int OrderLineNumber,
        string Name,
        decimal UnitPrice,
        int Quantity);
}
