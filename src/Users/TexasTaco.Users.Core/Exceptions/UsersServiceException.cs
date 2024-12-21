using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Users.Core.Exceptions
{
    public abstract class UsersServiceException(
        string message,
        ExceptionCategory exceptionCategory) : Exception(message)
    {
        public ExceptionCategory ExceptionCategory { get; } = exceptionCategory;
    }
}
