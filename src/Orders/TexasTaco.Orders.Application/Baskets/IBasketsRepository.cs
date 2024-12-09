using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Baskets
{
    public interface IBasketsRepository
    {
        Task<Basket?> GetBasketByCustomerId(CustomerId customerId);
        Task<IReadOnlyCollection<Basket>> GetBasketsWithProduct(ProductId productId);
        Task AddAsync(Basket basket);
        Task UpdateAsync(Basket basket);
    }
}
