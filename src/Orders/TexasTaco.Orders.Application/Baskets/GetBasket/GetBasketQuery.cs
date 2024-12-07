using MediatR;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Baskets.GetBasket
{
    public record GetBasketQuery(CustomerId CustomerId) : IRequest<Basket>;
}
