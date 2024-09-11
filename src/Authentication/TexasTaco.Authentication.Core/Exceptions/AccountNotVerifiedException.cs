using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Exceptions
{
    public class AccountNotVerifiedException(Account account) 
        : Exception($"Account with email {account.Email.Value} has not been verified yet.");
}
