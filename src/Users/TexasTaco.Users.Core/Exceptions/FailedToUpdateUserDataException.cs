using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Exceptions
{
    internal class FailedToUpdateUserDataException(User user, Exception innerException)
        : Exception($"Failed to update user {user.FullName} " +
            $"with account Id {user.AccountId}.", innerException);
}
