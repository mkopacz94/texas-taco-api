using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Exceptions
{
    public class AccountAssociatedWithTokenNotFoundException(VerificationToken token)
        : AuthenticationServiceException($"Account associated with {token.Token} does not exist.");
}
