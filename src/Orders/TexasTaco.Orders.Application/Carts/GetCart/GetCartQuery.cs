using MediatR;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Carts.GetCart
{
    public record GetCartQuery(CustomerId CustomerId) : IRequest<Cart>;
}
