using TexasTaco.Shared.Exceptions;
using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Exceptions
{
    public class UserNotFoundException : UsersServiceException
    {
        public UserNotFoundException(UserId id)
            : base($"User with ID {id.Value} does not exist.",
                ExceptionCategory.NotFound)
        {

        }

        public UserNotFoundException(AccountId id)
            : base($"User with account ID {id.Value} does not exist.",
                ExceptionCategory.NotFound)
        {

        }
    }
}
