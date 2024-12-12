namespace TexasTaco.Orders.Application.Customers.DTO
{
    public sealed record AddressDto(
        string AddressLine,
        string PostalCode,
        string City,
        string Country);
}
