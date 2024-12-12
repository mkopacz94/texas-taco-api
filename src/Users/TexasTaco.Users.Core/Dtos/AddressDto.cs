namespace TexasTaco.Users.Core.Dtos
{
    public sealed record AddressDto(
        string AddressLine,
        string PostalCode,
        string City,
        string Country);
}
