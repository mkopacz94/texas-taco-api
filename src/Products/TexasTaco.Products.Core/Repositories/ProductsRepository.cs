using Microsoft.EntityFrameworkCore;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    internal class ProductsRepository(ProductsDbContext _context) : IProductsRepository
    {
        public async Task AddAsync(Product product)
        {
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetAsync(ProductId id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(ProductId productId)
        {
            return await _context
                .Products
                .AnyAsync(p => p.Id == productId);
        }
    }
}
