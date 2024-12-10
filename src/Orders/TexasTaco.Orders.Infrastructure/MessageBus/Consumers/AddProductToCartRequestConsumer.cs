using Humanizer;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TexasTaco.Orders.Application.Carts;
using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Cart.Exceptions;
using TexasTaco.Shared.Errors;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Orders.Infrastructure.MessageBus.Consumers
{
    internal class AddProductToCartRequestConsumer(
        ICartService _cartService,
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

                var cart = await _cartService
                    .AddItemToCart(accountId, cartProduct);

                string productLocation = $"/api/v1/orders/cart/{cart.Id.Value}/products/{cartProduct.Id.Value}";
                response = new AddProductToCartResponse(
                    true,
                    HttpStatusCode.Created,
                    productLocation);

                await context.RespondAsync(response);
            }
            catch (CartProductException ex)
            {
                response = new AddProductToCartResponse(
                    false,
                    ex.ExceptionCategory.AsStatusCode(),
                    ErrorMessage: BuildErrorMessage(ex));
            }

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
