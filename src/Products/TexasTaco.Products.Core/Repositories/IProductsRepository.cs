using System.Linq.Expressions;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Shared.Pagination;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Core.Repositories
{
    public interface IProductsRepository
    {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<PagedResult<Product>> GetPagedProductsAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<Product, bool>>? filter);
        Task<Product?> GetAsync(ProductId id);
        Task UpdateAsync(Product product);
        Task<bool> AnyAsync(ProductId productId);
        Task DeleteAsync(ProductId productId);
    }
}
