﻿using TexasTaco.Orders.Application.Orders.DTO;

namespace TexasTaco.Orders.Application.Customers.DTO
{
    public sealed record CustomerDto(
        Guid Id,
        string EmailAddress,
        string? FirstName,
        string? LastName,
        AddressDto Address,
        int PointsCollected,
        List<OrderDto> Orders);
}
