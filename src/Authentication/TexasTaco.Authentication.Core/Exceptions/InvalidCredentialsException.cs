using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Exceptions
{
    public class InvalidCredentialsException() 
        : AuthenticationServiceException("Given credentials are invalid.")
    {
    }
}
