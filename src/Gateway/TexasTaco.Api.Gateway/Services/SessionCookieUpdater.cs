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

            string? accountId = context.Request.Cookies[CookiesNames.AccountId];
            string? sessionId = context.Request.Cookies[CookiesNames.SessionId];
            var savedSession = _currentSessionStorage.GetSavedSessionFromStorage();

            if (accountId is null || sessionId is null || savedSession is null)
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
                    setCookieHeader, accountId, sessionId, savedSession);

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
                    .Add(CreateNewSetCookieHeader(accountId, sessionId, savedSession));  
            }
        }

        private static Header CreateNewSetCookieHeader(string accountId, string sessionId, Session session)
        {
            var headerValues = new List<string>
            {
                $"session_id={sessionId};" +
                $"expires={session.ExpirationDate:ddd, dd MMM yyyy HH':'mm':'ss 'GMT'};" +
                $"path=/;" +
                $"secure;" +
                $"SameSite=Strict;" +
                $"httponly",

                $"account_id={accountId};" +
                $"expires={session.ExpirationDate:ddd, dd MMM yyyy HH':'mm':'ss 'GMT'};" +
                $"path=/;" +
                $"secure;" +
                $"SameSite=Strict;" +
                $"httponly"
            };

            return new Header(HeaderNames.SetCookie, headerValues);
        }

        private static Header CreateUpdatedSetCookieHeader(
            Header currentSetCookieHeader, string accountId, string sessionId, Session session)
        {
            var updatedCookies = new List<string>();
            var cookiesWithoutSessionCookies = currentSetCookieHeader
                .Values
                .Where(v => !v.Contains(CookiesNames.SessionId) 
                    && !v.Contains(CookiesNames.AccountId));

            updatedCookies.AddRange(cookiesWithoutSessionCookies);
            updatedCookies.Add($"session_id={sessionId};" +
                $"expires={session.ExpirationDate:ddd, dd MMM yyyy HH':'mm':'ss 'GMT'};" +
                $"path=/;" +
                $"secure;" +
                $"httponly");
            updatedCookies.Add($"account_id={accountId};" +
               $"expires={session.ExpirationDate:ddd, dd MMM yyyy HH':'mm':'ss 'GMT'};" +
               $"path=/;" +
               $"secure;" +
               $"httponly");

            return new Header(HeaderNames.SetCookie, updatedCookies);
        }
    }
}
