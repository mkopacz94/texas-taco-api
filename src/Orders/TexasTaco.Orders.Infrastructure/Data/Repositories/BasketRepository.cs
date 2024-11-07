using TexasTaco.Orders.Application.Baskets;
using TexasTaco.Orders.Domain.Basket;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Infrastructure.Data.Repositories
{
    internal class BasketRepository(OrdersDbContext _context) : IBasketRepository
    {
        public async Task AddAsync(Basket basket)
        {
            await _context.AddAsync(basket);
            await _context.SaveChangesAsync();
        }
    }
}
