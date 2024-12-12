namespace TexasTaco.Users.Core.Dtos
{
    public sealed record UserDto(
        Guid Id,
        string Email,
        string? FirstName,
        string? LastName,
        AddressDto Address);
}
