namespace TexasTaco.Authentication.Api.Exceptions
{
    internal class ClaimNotExistException(string claimType) 
        : Exception($"Claim of type {claimType} not found.")
    {
    }
}
