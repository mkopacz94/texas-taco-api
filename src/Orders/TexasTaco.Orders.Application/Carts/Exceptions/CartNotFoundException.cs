using TexasTaco.Orders.Domain.Cart;
using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Application.Carts.Exceptions
{
    public class CartNotFoundException : OrdersServiceException
    {
        public CartNotFoundException(CustomerId customerId) : base(
            $"Cart for customer with ID {customerId.Value} does not exist.",
            ExceptionCategory.NotFound)
        { }

        public CartNotFoundException(CartId cartId) : base(
           $"Cart with ID {cartId.Value} does not exist.",
           ExceptionCategory.NotFound)
        { }
    }
}
