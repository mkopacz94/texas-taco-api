using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Shared.Pagination;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    internal class ProductsRepository(ProductsDbContext _context)
        : IProductsRepository
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

        public async Task<PagedResult<Product>> GetPagedProductsAsync(
            int pageNumber,
            int pageSize,
            string? searchQuery)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var filter = BuildFilterForSearchQuery(searchQuery);
                query = query.Where(filter);
            }

            int totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new(items, totalCount, pageSize, pageNumber);
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

        public async Task DeleteAsync(ProductId productId)
        {
            var productToDelete = await _context
                .Products
                .FirstOrDefaultAsync(p => p.Id == productId)
                ?? throw new ProductNotFoundException(productId);

            _context.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }

        private static Expression<Func<Product, bool>> BuildFilterForSearchQuery(
            string searchQuery)
        {
            string normalizedQuery = searchQuery.ToLower();

            bool isQueryDecimal = decimal.TryParse(
                searchQuery,
                out decimal parsedDecimalQuery);

            return p => (p.Name != null
                    && p.Name.ToLower().Contains(normalizedQuery))
                || (p.ShortDescription != null
                    && p.ShortDescription.ToLower().Contains(normalizedQuery))
                || (isQueryDecimal && Math.Floor(p.Price) == Math.Floor(parsedDecimalQuery));
        }
    }
}
