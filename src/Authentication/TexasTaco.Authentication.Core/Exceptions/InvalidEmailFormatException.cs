namespace TexasTaco.Authentication.Core.Exceptions
{
    public class InvalidEmailFormatException(string message)
        : AuthenticationServiceException(message);
}
