using Microsoft.EntityFrameworkCore;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Repositories
{
    internal class PrizesRepository(ProductsDbContext _context) : IPrizesRepository
    {
        public async Task<IEnumerable<Prize>> GetAllAsync()
        {
            return await _context.Prizes.ToListAsync();
        }
    }
}
