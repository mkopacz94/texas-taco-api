using MediatR;
using TexasTaco.Orders.Application.Baskets.Exceptions;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Basket;

namespace TexasTaco.Orders.Application.Baskets.GetBasket
{
    internal class GetBasketQueryHandler(
        ICustomersRepository _customersRepository,
        IBasketsRepository _basketsRepository) : IRequestHandler<GetBasketQuery, Basket>
    {
        public async Task<Basket> Handle(
            GetBasketQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _customersRepository
                .GetByIdAsync(request.CustomerId)
                ?? throw new CustomerNotFoundException(request.CustomerId);

            var basket = await _basketsRepository
                .GetBasketByCustomerId(customer.Id);

            return basket is null
                ? throw new BasketNotFoundException(request.CustomerId)
                : basket;
        }
    }
}
