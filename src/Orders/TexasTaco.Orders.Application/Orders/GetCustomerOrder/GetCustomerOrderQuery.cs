using MediatR;
using TexasTaco.Orders.Application.Orders.DTO;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Orders.GetCustomerOrder
{
    public sealed record GetCustomerOrderQuery(CustomerId CustomerId)
        : IRequest<OrderDto>;
}
