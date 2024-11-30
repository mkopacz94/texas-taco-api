using Microsoft.EntityFrameworkCore;
using TexasTaco.Orders.Application.Baskets;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class BasketsRepository(OrdersDbContext _context) : IBasketsRepository
    {
        public async Task AddAsync(Basket basket)
        {
            await _context.AddAsync(basket);
            await _context.SaveChangesAsync();
        }

        public async Task<Basket?> GetBasketByCustomerId(CustomerId customerId)
        {
            return await _context
                .Baskets
                .FirstOrDefaultAsync(b => b.CustomerId == customerId);
        }

        public async Task UpdateAsync(Basket basket)
        {
            _context.Update(basket);
            await _context.SaveChangesAsync();
        }
    }
}
