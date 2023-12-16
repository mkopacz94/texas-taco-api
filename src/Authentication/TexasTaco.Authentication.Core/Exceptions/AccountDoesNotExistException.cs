using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Exceptions
{
    public class AccountDoesNotExistException(AccountId id)
        : AuthenticationServiceException($"Account with id {id.Value} does not exist.");
}
