using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Api.Services
{
    public interface IClaimsService
    {
        Task SetAccountClaims(Account account);
    }
}
