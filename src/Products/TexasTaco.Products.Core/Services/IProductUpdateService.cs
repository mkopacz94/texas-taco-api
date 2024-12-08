using TexasTaco.Products.Core.DTO;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Products.Core.Services
{
    public interface IProductUpdateService
    {
        Task UpdateProductAsync(ProductId productId, ProductInputDto updateData);
    }
}
