using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<Account> CreateAccount(EmailAddress email, Role role, string password);
        Task<Account> AuthenticateAccount(EmailAddress email, string password);
    }
}
