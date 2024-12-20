using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Exceptions
{
    public class CategoryNotFoundException(CategoryId categoryId)
        : ProductsServiceException($"Category with " +
            $"ID {categoryId.Value} does not exist.");
}
