using MediatR;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Application.Carts.Exceptions;
using TexasTaco.Orders.Application.Carts.Mapping;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;

namespace TexasTaco.Orders.Application.Carts.GetCart
{
    internal class GetCartQueryHandler(
        ICustomersRepository customersRepository,
        ICartsRepository cartsRepository) : IRequestHandler<GetCartQuery, CartDto>
    {
        private readonly ICustomersRepository _customersRepository = customersRepository;
        private readonly ICartsRepository _cartsRepository = cartsRepository;

        public async Task<CartDto> Handle(
            GetCartQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _customersRepository
                .GetByIdAsync(request.CustomerId)
                ?? throw new CustomerNotFoundException(request.CustomerId);

            var cart = await _cartsRepository
                .GetCartByCustomerIdAsync(customer.Id);

            return cart is null
                ? throw new CartNotFoundException(request.CustomerId)
                : CartMap.Map(cart);
        }
    }
}
