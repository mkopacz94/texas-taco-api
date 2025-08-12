using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;

namespace TexasTaco.Products.Core.Mapping
{
    public static class ProductMap
    {
        public static ProductDto Map(Product product)
        {
            return new(
                product.Id.Value,
                product.Name,
                product.ShortDescription,
                product.Recommended,
                product.Price,
                new PictureDto(
                    product.Picture.Id.Value,
                    product.Picture.Url),
                product.Picture.ThumbnailUrl,
                product.Category.Name);
        }
    }
}
