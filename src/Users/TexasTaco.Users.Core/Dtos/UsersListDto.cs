namespace TexasTaco.Users.Core.Dtos
{
    public sealed record UsersListDto(
        Guid Id,
        string Email,
        string? FullName,
        AddressDto Address,
        int PointsCollected);
}
