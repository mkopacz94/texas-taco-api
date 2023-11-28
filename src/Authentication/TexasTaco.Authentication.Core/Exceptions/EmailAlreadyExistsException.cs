using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Exceptions
{
    public class EmailAlreadyExistsException(EmailAddress email) 
        : AuthenticationServiceException($"{email.Value} email address has already been registered.")
    {
    }
}
