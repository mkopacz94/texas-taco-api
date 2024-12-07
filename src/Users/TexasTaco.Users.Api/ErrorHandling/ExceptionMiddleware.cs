using System.Net;
using TexasTaco.Shared.Errors;

namespace TexasTaco.Users.Api.ErrorHandling
{
    internal sealed class ExceptionMiddleware(
        ILogger<ExceptionMiddleware> _logger) : IMiddleware
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
                _ => (HttpStatusCode.InternalServerError, new ErrorMessage("server_error", "There was an internal server error."))
            };

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(message);
        }
    }
}
