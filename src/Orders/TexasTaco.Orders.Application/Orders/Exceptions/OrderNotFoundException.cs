using TexasTaco.Orders.Domain.Customers;
using TexasTaco.Orders.Domain.Orders;
using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Application.Orders.Exceptions
{
    public class OrderNotFoundException : OrdersServiceException
    {
        public OrderNotFoundException(CustomerId customerId) : base(
            $"Order for customer with ID {customerId.Value} does not exist.",
            ExceptionCategory.NotFound)
        { }

        public OrderNotFoundException(OrderId orderId) : base(
           $"Order with ID {orderId.Value} does not exist.",
           ExceptionCategory.NotFound)
        { }
    }
}
