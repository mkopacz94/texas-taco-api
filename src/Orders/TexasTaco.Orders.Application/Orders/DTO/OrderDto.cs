﻿using TexasTaco.Orders.Domain.Orders;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Application.Orders.DTO
{
    public sealed record OrderDto(
        Guid Id,
        Guid CustomerId,
        List<OrderLineDto> Lines,
        PaymentType PaymentType,
        PickupLocation PickupLocation,
        decimal TotalPrice,
        OrderStatus Status);
}
