﻿namespace TexasTaco.Products.Core.DTO
{
    public sealed record PrizeDto(
        Guid Id,
        Guid ProductId,
        string Name,
        int RequiredPointsAmount,
        string? PictureUrl);
}
