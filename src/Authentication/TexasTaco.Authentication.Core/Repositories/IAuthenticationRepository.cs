using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAuthenticationRepository
    {
        Task CreateAccount(EmailAddress email,  string password);
        Task<bool> EmailAlreadyExists(EmailAddress email);
    }
}
