using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Mapping
{
    public static class CategoryWithProductsMap
    {
        public static CategoryWithProductsDto Map(Category category)
        {
            var productsDto = category
                .Products
                .Select(ProductMap.Map)
                .ToList();

            return new(
                category.Id.Value,
                category.Name,
                productsDto);
        }
    }
}
