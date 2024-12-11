using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.Carts;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class CheckoutCartsRepository(OrdersDbContext _context)
        : ICheckoutCartsRepository
    {
        public async Task AddAsync(CheckoutCart checkoutCart)
        {
            await _context.AddAsync(checkoutCart);
            await _context.SaveChangesAsync();
        }

        public async Task<CheckoutCart?> GetAsync(CheckoutCartId id)
        {
            return await _context
                .CheckoutCarts
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(CheckoutCart checkoutCart)
        {
            _context.Update(checkoutCart);
            await _context.SaveChangesAsync();
        }
    }
}
