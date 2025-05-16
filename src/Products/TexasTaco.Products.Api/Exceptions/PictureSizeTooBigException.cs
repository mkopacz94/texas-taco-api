using TexasTaco.Products.Core.Exceptions;

namespace TexasTaco.Products.Api.Exceptions
{
    public class PictureSizeTooBigException(int sizeInKB, int maximumSizeInKB)
        : ProductsServiceException(
            $"Picture size is too big ({sizeInKB} KB). " +
            $"Maximum file size is {maximumSizeInKB} KB.");
}
