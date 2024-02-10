using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Api.Services
{
    public interface IClaimsService
    {
        Task SetAccountClaims(Account account);
    }
}
