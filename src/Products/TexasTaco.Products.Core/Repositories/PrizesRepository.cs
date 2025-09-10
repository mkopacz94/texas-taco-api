using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Pagination;

namespace TexasTaco.Products.Core.Repositories
{
    internal class PrizesRepository(ProductsDbContext _context)
        : IPrizesRepository
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

        public async Task<PagedResult<Prize>> GetPagedPrizesAsync(
            int pageNumber,
            int pageSize,
            string? searchQuery)
        {
            IQueryable<Prize> query = _context.Prizes;

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

        public async Task UpdateAsync(Prize prize)
        {
            _context.Update(prize);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PrizeId id)
        {
            var prizeToDelete = await _context
                .Prizes
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new PrizeNotFoundException(id);

            _context.Remove(prizeToDelete);
            await _context.SaveChangesAsync();
        }

        private static Expression<Func<Prize, bool>> BuildFilterForSearchQuery(
            string searchQuery)
        {
            string normalizedQuery = searchQuery.ToLower();

            bool isQueryDecimal = decimal.TryParse(
                searchQuery,
                out decimal parsedDecimalQuery);

            return p => (p.Name != null
                    && p.Name.ToLower().Contains(normalizedQuery))
                || (p.Product != null
                    && p.Product.Name != null
                    && p.Product.Name.ToLower().Contains(normalizedQuery))
                || (p.Product != null
                    && p.Product.ShortDescription != null
                    && p.Product.ShortDescription.ToLower().Contains(normalizedQuery));
        }
    }
}
