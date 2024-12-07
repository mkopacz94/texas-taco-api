using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Baskets
{
    internal class BasketService(
        ICustomersRepository customersRepository,
        IBasketsRepository basketRepository,
        ILogger<BasketService> logger) : IBasketService
    {
        private readonly ICustomersRepository _customersRepository = customersRepository;
        private readonly IBasketsRepository _basketRepository = basketRepository;
        private readonly ILogger<BasketService> _logger = logger;

        public async Task<Basket> AddItemToBasket(AccountId accountId, BasketItem item)
        {
            var customer = await _customersRepository
                .GetByAccountIdAsync(accountId)
                ?? throw new CustomerWithAccountIdNotFoundException(accountId);

            var customerBasket = await _basketRepository
                .GetBasketByCustomerId(customer.Id);

            if (customerBasket is null)
            {
                _logger.LogDebug(
                    "Customer's {customerId} basket not found. Creating a new one.",
                    customer.Id.Value);

                customerBasket = new Basket(customer.Id);
                await _basketRepository.AddAsync(customerBasket);
            }

            _logger.LogDebug(
                "Customer's {customerId} basket has been found. The item will be added to it.",
                customer.Id.Value);

            customerBasket.AddProduct(item);
            await _basketRepository.UpdateAsync(customerBasket);

            return customerBasket;
        }
    }
}
