﻿namespace TexasTaco.Users.Core.Dtos
{
    public sealed record UserDto(
        Guid Id,
        string Email,
        string? FullName,
        AddressDto Address,
        int PointsCollected);
}
