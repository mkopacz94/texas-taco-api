using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Exceptions
{
    public sealed class ProductNotFoundException(ProductId productId) 
        : ProductsServiceException($"Product with Id {productId.Value} does not exist.");
}
