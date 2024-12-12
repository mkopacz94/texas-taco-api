using MediatR;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Carts.GetCart
{
    public record GetCartQuery(CustomerId CustomerId) : IRequest<CartDto>;
}
