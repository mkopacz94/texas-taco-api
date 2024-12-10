using System.Net;

namespace TexasTaco.Shared.Exceptions
{
    public static class ExceptionMapping
    {
        public static HttpStatusCode AsStatusCode(this ExceptionCategory? exceptionCategory)
            => MapToStatusCode(exceptionCategory);

        public static HttpStatusCode AsStatusCode(this ExceptionCategory exceptionCategory)
            => MapToStatusCode(exceptionCategory);

        private static HttpStatusCode MapToStatusCode(ExceptionCategory? exceptionCategory)
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
