using MediatR;
using TexasTaco.Orders.Application.Carts.Exceptions;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.GetCart
{
    internal class GetCartQueryHandler(
        ICustomersRepository customersRepository,
        ICartsRepository cartsRepository) : IRequestHandler<GetCartQuery, Cart>
    {
        private readonly ICustomersRepository _customersRepository = customersRepository;
        private readonly ICartsRepository _cartsRepository = cartsRepository;

        public async Task<Cart> Handle(
            GetCartQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _customersRepository
                .GetByIdAsync(request.CustomerId)
                ?? throw new CustomerNotFoundException(request.CustomerId);

            var cart = await _cartsRepository
                .GetCartByCustomerId(customer.Id);

            return cart is null
                ? throw new CartNotFoundException(request.CustomerId)
                : cart;
        }
    }
}
