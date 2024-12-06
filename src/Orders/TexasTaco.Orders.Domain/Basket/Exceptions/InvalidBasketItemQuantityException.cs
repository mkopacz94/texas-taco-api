namespace TexasTaco.Orders.Domain.Basket.Exceptions
{
    public class InvalidBasketItemQuantityException(int quantity)
        : Exception($"Quantity {quantity} is invalid value.");
}
