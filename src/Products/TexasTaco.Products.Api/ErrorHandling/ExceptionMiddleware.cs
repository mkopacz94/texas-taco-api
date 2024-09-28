using Humanizer;
using System.Net;
using TexasTaco.Products.Core.Exceptions;
using TexasTaco.Shared.Errors;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Products.Api.ErrorHandling
{
    internal sealed class ExceptionMiddleware(ILogger<ExceptionMiddleware> _logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{message}", ex.Message);
                await HandleExceptionAsync(ex, context);
            }
        }

        private static async Task HandleExceptionAsync(Exception ex, HttpContext context)
        {
            var (statusCode, message) = ex switch
            {
                ProductsServiceException => (HttpStatusCode.BadRequest, CreateErrorMessage(ex)),
                _ => (HttpStatusCode.InternalServerError, new ErrorMessage("server_error", "There was an internal server error."))
            };

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(message);
        }

        private static ErrorMessage CreateErrorMessage(Exception ex)
            => new(ex.GetType().Name.Underscore().Replace("_exception", string.Empty), ex.Message);
    }
}
