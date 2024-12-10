using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Orders.Shared.Exceptions
{
    public class OrdersServiceException(
        string message,
        ExceptionCategory exceptionCategory) : Exception(message)
    {
        public ExceptionCategory ExceptionCategory { get; } = exceptionCategory;
    }
}
