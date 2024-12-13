using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.Carts;
using TexasTaco.Orders.Application.Carts.Exceptions;
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

        public async Task<Cart?> GetCartAsync(CartId id)
        {
            return await _context
                .Carts
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cart?> GetCartByCustomerIdAsync(CustomerId customerId)
        {
            return await _context
                .Carts
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<IReadOnlyCollection<Cart>> GetCartsWithProductAsync(ProductId productId)
        {
            return await _context
                .Carts
                .Where(c => c.Products.Any(cp => cp.ProductId == productId))
                .ToListAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CartId id)
        {
            var cartToRemove = await _context
                .Carts
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cartToRemove is null)
            {
                throw new CartNotFoundException(id);
            }

            _context.Remove(cartToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
