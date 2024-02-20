using TexasTaco.Api.Gateway.Services;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;
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
            string? sessionId = _contextAccessor
                .HttpContext?
                .Request
                .Cookies[CookiesNames.SessionId];

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

            ExtendSessionCookie(
                new SessionId(Guid.Parse(sessionId)),
                session.ExpirationDate);

            return true;
        }

        private void ExtendSessionCookie(SessionId sessionId, DateTime expirationDate)
        {
            _cookieService.SetCookie(
                CookiesNames.SessionId, 
                sessionId.Value.ToString(),
                new CookieOptions
                {
                    Expires = new DateTimeOffset(expirationDate),
                    HttpOnly = true,
                    Secure = true
                });
        }
    }
}
