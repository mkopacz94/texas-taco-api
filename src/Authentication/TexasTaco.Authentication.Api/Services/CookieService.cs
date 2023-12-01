
using TexasTaco.Authentication.Api.Abstractions;

namespace TexasTaco.Authentication.Api.Services
{
    public class CookieService(IHttpContextAccessor _contextAccessor) : ICookieService
    {
        public string? GetCookie(string cookieName)
        {
            var context = _contextAccessor.HttpContext!;
            return context.Request.Cookies[cookieName];
        }

        public void SetCookie(string cookieName, string value, CookieOptions options)
        {
            var context = _contextAccessor.HttpContext!;
            context.Response.Cookies.Append(cookieName, value, options);
        }
    }
}
