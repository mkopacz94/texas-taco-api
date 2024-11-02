namespace TexasTaco.Orders.Domain.Basket.Exceptions
{
    internal class InvalidBasketItemQuantityException(int quantity)
        : Exception($"Quantity {quantity} is invalid value.");
}
