using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Api.Services
{
    public interface IClaimsManager
    {
        Task SetAccountClaims(Account account);
    }
}
