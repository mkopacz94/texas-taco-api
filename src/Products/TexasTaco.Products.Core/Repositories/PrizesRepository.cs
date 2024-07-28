using Microsoft.EntityFrameworkCore;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    internal class PrizesRepository(ProductsDbContext _context) : IPrizesRepository
    {
        public async Task<IEnumerable<Prize>> GetAllAsync()
        {
            return await _context.Prizes.ToListAsync();
        }

        public async Task AddAsync(Prize prize)
        {
            await _context.AddAsync(prize);
            await _context.SaveChangesAsync();
        }

        public async Task<Prize?> GetAsync(PrizeId id)
        {
            return await _context.Prizes
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
