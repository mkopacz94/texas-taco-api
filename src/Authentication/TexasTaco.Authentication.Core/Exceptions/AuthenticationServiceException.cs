namespace TexasTaco.Authentication.Core.Exceptions
{
    public abstract class AuthenticationServiceException : Exception
    {
        protected AuthenticationServiceException(string? message) : base(message)
        {
        }
    }
}
