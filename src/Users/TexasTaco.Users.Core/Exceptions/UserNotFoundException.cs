using TexasTaco.Shared.Exceptions;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Exceptions
{
    public class UserNotFoundException(UserId id)
        : UsersServiceException(
            $"User with ID {id.Value} does not exist.",
            ExceptionCategory.NotFound)
    {
    }
}
