using System.Net;
using TexasTaco.Orders.Shared.Exceptions;

namespace TexasTaco.Orders.Api.ErrorHandling
{
    internal static class ExceptionMapping
    {
        internal static HttpStatusCode AsStatusCode(this ExceptionCategory exceptionCategory)
        {
            return exceptionCategory switch
            {
                ExceptionCategory.BadRequest => HttpStatusCode.BadRequest,
                ExceptionCategory.NotFound => HttpStatusCode.NotFound,
                ExceptionCategory.ValidationError => HttpStatusCode.UnprocessableEntity,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}
