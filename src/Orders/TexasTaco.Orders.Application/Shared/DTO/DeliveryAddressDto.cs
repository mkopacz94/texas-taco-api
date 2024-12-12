namespace TexasTaco.Orders.Application.Shared.DTO
{
    public sealed record DeliveryAddressDto(
        string ReceiverFullName,
        string AddressLine,
        string PostalCode,
        string City);
}
