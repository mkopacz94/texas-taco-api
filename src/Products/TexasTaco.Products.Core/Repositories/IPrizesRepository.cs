using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Repositories
{
    public interface IPrizesRepository
    {
        Task<IEnumerable<Prize>> GetAllAsync();
    }
}
