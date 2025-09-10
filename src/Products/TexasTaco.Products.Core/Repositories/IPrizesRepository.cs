using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Pagination;

namespace TexasTaco.Products.Core.Repositories
{
    public interface IPrizesRepository
    {
        Task<IEnumerable<Prize>> GetAllAsync();
        Task AddAsync(Prize prize);
        Task<Prize?> GetAsync(PrizeId id);
        Task<PagedResult<Prize>> GetPagedPrizesAsync(
            int pageNumber,
            int pageSize,
            string? searchQuery);
        Task UpdateAsync(Prize prize);
        Task DeleteAsync(PrizeId id);
    }
}
