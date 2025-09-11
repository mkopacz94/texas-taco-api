using Microsoft.EntityFrameworkCore;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    internal class CategoriesRepository(ProductsDbContext context)
        : ICategoriesRepository
    {
        private readonly ProductsDbContext _context = context;

        public async Task AddAsync(Category category)
        {
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context
                .Categories
                .ToListAsync();
        }

        public async Task<Category?> GetAsync(CategoryId id)
        {
            return await _context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteAsync(CategoryId id)
        {
            var categoryToDelete = await _context
                .Categories
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new CategoryNotFoundException(id);

            _context.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
