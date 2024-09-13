using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Exceptions
{
    public class AccountAlreadyVerifiedException(Account account)
        : AuthenticationServiceException($"Account with email {account.Email.Value} has already been verified yet.");
}

