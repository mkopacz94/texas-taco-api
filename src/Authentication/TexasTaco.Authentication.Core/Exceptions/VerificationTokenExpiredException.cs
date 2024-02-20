namespace TexasTaco.Authentication.Core.Exceptions
{
    public class VerificationTokenExpiredException() 
        : AuthenticationServiceException("Verification token already expired.");
}
