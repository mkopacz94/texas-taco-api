using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Api.Services
{
    public class CookieClaimsService(IHttpContextAccessor _httpContextAccessor) : IClaimsService
    {
        public async Task SetAccountClaims(Account account)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, account.Email.Value),
                new("AccountId", account.Id.Value.ToString()),
                new(ClaimTypes.Role, account.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var context = _httpContextAccessor.HttpContext!;
            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties());
        }
    }
}
