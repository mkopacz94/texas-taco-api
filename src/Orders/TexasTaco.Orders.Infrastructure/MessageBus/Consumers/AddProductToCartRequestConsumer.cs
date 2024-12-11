using Humanizer;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TexasTaco.Orders.Application.Carts.AddProductToCart;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Cart.Exceptions;
using TexasTaco.Shared.Errors;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.MessageBus.Consumers
{
    internal class AddProductToCartRequestConsumer(
        IMediator _mediator,
        ILogger<AddProductToCartRequestConsumer> _logger)
        : IConsumer<AddProductToCartRequest>
    {
        public async Task Consume(ConsumeContext<AddProductToCartRequest> context)
        {
            _logger.LogInformation(
                "Received AddProductToCartRequest. {jsonObject}",
                JsonSerializer.Serialize(context.Message));

            var busMessage = context.Message;
            AddProductToCartResponse response;

            try
            {
                var cartProduct = new CartProduct(
                    busMessage.ProductId,
                    busMessage.Name,
                    busMessage.Price,
                    busMessage.PictureUrl,
                    busMessage.Quantity);

                var accountId = new AccountId(busMessage.AccountId);

                var command = new AddProductToCartCommand(accountId, cartProduct);
                var cart = await _mediator.Send(command);

                string productLocation = $"/api/v1/orders/cart/{cart.Id.Value}/products/{cartProduct.Id.Value}";
                response = new AddProductToCartResponse(
                    true,
                    HttpStatusCode.Created,
                    productLocation);

                await context.RespondAsync(response);
            }
            catch (CartProductException ex)
            {
                _logger.LogError(ex, "{exceptionMessage}", ex.Message);

                response = new AddProductToCartResponse(
                    false,
                    ex.ExceptionCategory.AsStatusCode(),
                    ErrorMessage: BuildErrorMessage(ex));
            }

            _logger.LogInformation(
                "Responded with AddProductToCartResponse. {jsonObject}",
                JsonSerializer.Serialize(response));

            await context.RespondAsync(response);
        }

        private static ErrorMessage BuildErrorMessage(Exception ex)
        {
            string errorCode = ex
                .GetType()
                .Name
                .Underscore()
                .Replace("_exception", string.Empty);

            return new ErrorMessage(
                errorCode,
                ex.Message);
        }
    }
}
