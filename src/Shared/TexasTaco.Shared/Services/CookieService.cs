using Microsoft.AspNetCore.Http;

namespace TexasTaco.Shared.Services
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

        public void MakeCookieExpired(string cookieName)
        {
            var context = _contextAccessor.HttpContext!;
            var cookieValue = context.Request.Cookies[cookieName];

            if (cookieValue is null)
            {
                return;
            }

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };

            context.Response.Cookies.Append(cookieName, cookieValue, cookieOptions);
        }
    }
}
