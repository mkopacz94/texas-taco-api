using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Mapping
{
    public static class CategoryMap
    {
        public static CategoryDto Map(Category category)
        {
            return new(
                category.Id.Value,
                category.Name);
        }
    }
}
