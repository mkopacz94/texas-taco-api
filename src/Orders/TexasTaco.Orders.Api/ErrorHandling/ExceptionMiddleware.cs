
using Humanizer;
using System.Net;
using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Errors;

namespace TexasTaco.Orders.Api.ErrorHandling
{
    internal sealed class ExceptionMiddleware(
        ILogger<ExceptionMiddleware> _logger) : IMiddleware
    {
        private const string SomethingWentWrong = "Something went wrong!";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await WriteErrorToResponse(ex, context);
            }
        }

        private async Task WriteErrorToResponse(Exception ex, HttpContext context)
        {
            if (ex is OrdersServiceException ordersException)
            {
                context.Response.StatusCode = (int)ordersException.ExceptionCategory.AsStatusCode();
                await context.Response.WriteAsJsonAsync(CreateErrorMessage(ordersException));

                return;
            }

            _logger.LogError(ex, "{message}", ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorMessage = new ErrorMessage("server_error", SomethingWentWrong);
            await context.Response.WriteAsJsonAsync(errorMessage);
        }

        private static ErrorMessage CreateErrorMessage(OrdersServiceException ex)
        {
            var errorCode = ex
                .GetType()
                .Name
                .Underscore()
                .Replace("_exception", string.Empty);

            return new(errorCode, ex.Message);
        }
    }
}
