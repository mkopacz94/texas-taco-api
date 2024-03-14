using Microsoft.Net.Http.Headers;
using Ocelot.Middleware;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Api.Gateway.Services
{
    internal class SessionCookieUpdater(ICurrentSessionStorage _currentSessionStorage) 
        : ISessionCookieUpdater
    {
        public void UpdateSessionCookie(HttpContext context)
        {
            var setCookieHeader = context.Items
                .DownstreamResponse()
                .Headers
                .FirstOrDefault(h => h.Key == HeaderNames.SetCookie);

            string? sessionId = context.Request.Cookies[CookiesNames.SessionId];
            var savedSession = _currentSessionStorage.GetSavedSessionFromStorage();

            if (sessionId is null || savedSession is null)
            {
                return;
            }

            if (setCookieHeader != null)
            {
                context.Items
                    .DownstreamResponse()
                    .Headers
                    .Remove(setCookieHeader);

                var updatedSetCookieHeader = CreateUpdatedSetCookieHeader(
                    setCookieHeader, sessionId, savedSession);

                context.Items
                    .DownstreamResponse()
                    .Headers
                    .Add(updatedSetCookieHeader);
            }
            else
            {
                context.Items
                    .DownstreamResponse()
                    .Headers
                    .Add(CreateNewSetCookieHeader(sessionId, savedSession));  
            }
        }

        private static Header CreateNewSetCookieHeader(string sessionId, Session session)
        {
            var headerValues = new List<string>
            {
                $"session_id={sessionId};" +
                $"expires={session.ExpirationDate:ddd, dd MMM yyyy HH':'mm':'ss 'GMT'};" +
                $"path=/;" +
                $"secure;" +
                $"httponly"
            };

            return new Header(HeaderNames.SetCookie, headerValues);
        }

        private static Header CreateUpdatedSetCookieHeader(
            Header currentSetCookieHeader, string sessionId, Session session)
        {
            var updatedCookies = new List<string>();
            var cookiesWithoutSessionIdCookie = currentSetCookieHeader
                .Values
                .Where(v => !v.Contains(CookiesNames.SessionId));

            updatedCookies.AddRange(cookiesWithoutSessionIdCookie);
            updatedCookies.Add($"session_id={sessionId};" +
                $"expires={session.ExpirationDate:ddd, dd MMM yyyy HH':'mm':'ss 'GMT'};" +
                $"path=/;" +
                $"secure;" +
                $"httponly");

            return new Header(HeaderNames.SetCookie, updatedCookies);
        }
    }
}
