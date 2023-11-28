using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAuthenticationRepository
    {
        Task CreateAccount(EmailAddress email, Role role, string password);
        Task<SessionId> AuthenticateAccount(EmailAddress email, string password);
    }
}
