using TexasTaco.Api.Gateway.Services;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Api.Gateway.Clients
{
    public class AuthenticationClient(
        IHttpContextAccessor _contextAccessor,
        ICookieService _cookieService, 
        HttpClient _client)
    {
        public async Task<bool> UserSessionValid()
        {
            string? sessionId = _contextAccessor.HttpContext?.Request.Cookies[CookiesNames.SessionId];

            if(string.IsNullOrWhiteSpace(sessionId))
            {
                return false;
            }

            var session = await _client
                .GetFromJsonAsync<Session>($"session-valid?sessionId={sessionId}");

            if(session is null)
            {
                return false;
            }

            _cookieService.SetCookie(CookiesNames.SessionId, sessionId,
                new CookieOptions
                {
                    Expires = new DateTimeOffset(session.ExpirationDate),
                    HttpOnly = true,
                    Secure = true
                });

            return true;
        }
    }
}
