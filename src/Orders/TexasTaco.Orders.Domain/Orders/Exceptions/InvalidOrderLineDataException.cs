using TexasTaco.Orders.Shared.Exceptions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Domain.Orders.Exceptions
{
    public class InvalidOrderLineDataException(string message)
        : OrdersServiceException(message, ExceptionCategory.ValidationError);
}
