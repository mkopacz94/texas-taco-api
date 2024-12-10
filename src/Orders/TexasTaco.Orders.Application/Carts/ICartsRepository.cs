using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Carts
{
    public interface ICartsRepository
    {
        Task<Cart?> GetCartByCustomerId(CustomerId customerId);
        Task<IReadOnlyCollection<Cart>> GetCartsWithProduct(ProductId productId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
    }
}
