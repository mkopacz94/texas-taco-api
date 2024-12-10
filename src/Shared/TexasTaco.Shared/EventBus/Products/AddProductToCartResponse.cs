using System.Net;
using TexasTaco.Shared.Errors;

namespace TexasTaco.Shared.EventBus.Products
{
    public sealed record AddProductToCartResponse(
        bool IsSuccess,
        HttpStatusCode StatusCode,
        string? ProductLocation = null,
        ErrorMessage? ErrorMessage = null);
}
