using MediatR;
using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Carts.DTO;
using TexasTaco.Orders.Application.Carts.Mapping;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Cart;

namespace TexasTaco.Orders.Application.Carts.AddProductToCart
{
    internal sealed class AddProductToCartCommandHandler(
        ICustomersRepository customersRepository,
        ICartsRepository cartsRepository,
        ILogger<AddProductToCartCommandHandler> logger)
        : IRequestHandler<AddProductToCartCommand, CartDto>
    {
        private readonly ICustomersRepository _customersRepository = customersRepository;
        private readonly ICartsRepository _cartsRepository = cartsRepository;
        private readonly ILogger<AddProductToCartCommandHandler> _logger = logger;

        public async Task<CartDto> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customersRepository
                .GetByAccountIdAsync(request.AccountId)
                ?? throw new CustomerWithAccountIdNotFoundException(request.AccountId);

            var customerCart = await _cartsRepository
                .GetCartByCustomerIdAsync(customer.Id);

            if (customerCart is null)
            {
                _logger.LogDebug(
                    "Customer's {customerId} cart not found. Creating a new one.",
                    customer.Id.Value);

                customerCart = new Cart(customer.Id);
                await _cartsRepository.AddAsync(customerCart);
            }

            _logger.LogDebug(
                "Customer's {customerId} cart has been found. The item will be added to it.",
                customer.Id.Value);

            customerCart.AddProduct(request.Item);
            await _cartsRepository.UpdateAsync(customerCart);

            return CartMap.Map(customerCart);
        }
    }
}
