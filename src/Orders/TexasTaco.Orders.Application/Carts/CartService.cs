﻿using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Carts
{
    internal class CartService(
        ICustomersRepository customersRepository,
        ICartsRepository cartsRepository,
        ILogger<CartService> logger) : ICartService
    {
        private readonly ICustomersRepository _customersRepository = customersRepository;
        private readonly ICartsRepository _cartsRepository = cartsRepository;
        private readonly ILogger<CartService> _logger = logger;

        public async Task<Cart> AddItemToCart(AccountId accountId, CartProduct item)
        {
            var customer = await _customersRepository
                .GetByAccountIdAsync(accountId)
                ?? throw new CustomerWithAccountIdNotFoundException(accountId);

            var customerCart = await _cartsRepository
                .GetCartByCustomerId(customer.Id);

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

            customerCart.AddProduct(item);
            await _cartsRepository.UpdateAsync(customerCart);

            return customerCart;
        }
    }
}
