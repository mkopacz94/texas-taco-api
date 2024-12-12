using TexasTaco.Products.Core.ValueObjects;

namespace TexasTaco.Products.Core.Exceptions
{
    public class PrizeNotFoundException(PrizeId prizeId)
        : ProductsServiceException($"Prize with Id {prizeId.Value} does not exist.");
}
