namespace TexasTaco.Users.Core.Dtos
{
    public record UserDto(string Id, string Email, 
        string? FirstName, string? LastName, AddressDto Address);
}
