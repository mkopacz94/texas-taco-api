using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Application.Carts
{
    public interface ICartsRepository
    {
        Task<Cart?> GetCartAsync(CartId id);
        Task<Cart?> GetCartByCustomerIdAsync(CustomerId customerId);
        Task<IReadOnlyCollection<Cart>> GetCartsWithProductAsync(ProductId productId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(CartId id);
    }
}
