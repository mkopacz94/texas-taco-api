﻿using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Shared.EventBus.Products
{
    public sealed record AddProductToBasketRequest(
        Guid AccountId,
        ProductId ProductId,
        string Name,
        decimal Price,
        string? PictureUrl,
        int Quantity);
}
