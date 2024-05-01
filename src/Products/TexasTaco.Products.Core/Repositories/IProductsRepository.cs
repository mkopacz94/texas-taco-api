using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    public interface IProductsRepository
    {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetAsync(ProductId id);
        Task UpdateAsync(Product product);
    }
}
