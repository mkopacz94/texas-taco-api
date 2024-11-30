using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customers;

namespace TexasTaco.Orders.Application.Baskets
{
    public interface IBasketsRepository
    {
        Task<Basket?> GetBasketByCustomerId(CustomerId customerId);
        Task AddAsync(Basket basket);
        Task UpdateAsync(Basket basket);
    }
}
