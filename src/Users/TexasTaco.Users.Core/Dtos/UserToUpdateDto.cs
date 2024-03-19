namespace TexasTaco.Users.Core.Dtos
{
    public record UserToUpdateDto(string FirstName, string LastName, AddressDto Address);
}
