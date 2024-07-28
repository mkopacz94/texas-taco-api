using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    public interface IPrizesRepository
    {
        Task<IEnumerable<Prize>> GetAllAsync();
        Task AddAsync(Prize prize);
        Task<Prize?> GetAsync(PrizeId id);
    }
}
