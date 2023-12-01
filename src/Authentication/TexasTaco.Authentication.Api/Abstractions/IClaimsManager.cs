using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Api.Abstractions
{
    public interface IClaimsManager
    {
        Task SetAccountClaims(Account account);
    }
}
