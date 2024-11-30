using Microsoft.Extensions.Logging;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Customers.Exceptions;
using TexasTaco.Orders.Domain.Basket;

namespace TexasTaco.Orders.Application.Baskets
{
    internal class BasketService(
        ICustomersRepository _customersRepository,
        IBasketsRepository _basketRepository,
        ILogger<BasketService> _logger) : IBasketService
    {
        public async Task<Basket> AddItemToBasket(Guid accountId, BasketItem item)
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
