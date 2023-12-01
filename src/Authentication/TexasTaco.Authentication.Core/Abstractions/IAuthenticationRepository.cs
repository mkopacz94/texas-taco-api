using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Abstractions
{
    public interface IAuthenticationRepository
    {
        Task CreateAccount(EmailAddress email, Role role, string password);
        Task<Account> AuthenticateAccount(EmailAddress email, string password);
    }
}
