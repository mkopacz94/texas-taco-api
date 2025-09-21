using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    public interface ICategoriesRepository
    {
        Task AddAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync(
            bool includeProducts = false);
        Task<Category?> GetAsync(
            CategoryId id,
            bool includeProducts = false);
        Task DeleteAsync(CategoryId id);
    }
}
