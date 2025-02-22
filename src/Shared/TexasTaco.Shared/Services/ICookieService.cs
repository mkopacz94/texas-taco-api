using Microsoft.AspNetCore.Http;

namespace TexasTaco.Shared.Services
{
    public interface ICookieService
    {
        void SetCookie(string cookieName, string value, CookieOptions options);
        string? GetCookie(string cookieName);
        void MakeCookieExpired(string cookieName);
    }
}
