using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Exceptions
{
    public sealed class PictureNotFoundException(PictureId pictureId)
        : ProductsServiceException($"Picture with Id {pictureId.Value} does not exist.");
}
