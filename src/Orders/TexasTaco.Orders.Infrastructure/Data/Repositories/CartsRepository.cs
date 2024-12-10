using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.Carts;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Infrastructure.Data.EF;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class CartsRepository(OrdersDbContext _context) : ICartsRepository
    {
        public async Task AddAsync(Cart cart)
        {
            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> GetCartByCustomerId(CustomerId customerId)
        {
            return await _context
                .Carts
                .FirstOrDefaultAsync(b => b.CustomerId == customerId);
        }

        public async Task<IReadOnlyCollection<Cart>> GetCartsWithProduct(ProductId productId)
        {
            return await _context
                .Carts
                .Where(b => b.Products.Any(bi => bi.ProductId == productId))
                .ToListAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
