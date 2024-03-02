using Ocelot.Middleware;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Model;
using TexasTaco.Api.Gateway.Services;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Api.Gateway.Authentication
{
    internal sealed class TexasTacoAuthenticationMiddleware
    {
        private readonly ICookieService _cookieService;
        private readonly IAuthenticationClient _authClient;
        private readonly RoutesConfiguration _routesConfiguration;

        public TexasTacoAuthenticationMiddleware(
            ICookieService cookieService,
            IAuthenticationClient authService,
            RoutesConfiguration routesConfiguration)
        {
            _cookieService = cookieService;
            _authClient = authService;
            _routesConfiguration = routesConfiguration;
        }

        public async Task InvokeAsync(HttpContext context, Func<Task> next)
        {
            string requestPath = context.Request.Path.Value!;

            if(IsNonAuthenticatedRoute(requestPath))
            {
                await next();
                return;
            }

            string? sessionId = context
                .Request
                .Cookies[CookiesNames.SessionId];

            var session = await _authClient.GetSession(sessionId);

            if(session != null)
            {
                UpdateSessionCookie(
                   new SessionId(Guid.Parse(sessionId!)),
                   session.ExpirationDate);

                await next();
                return;
            }

            context.Items.SetError(new UnauthenticatedError("Authentication failed"));
        }

        private bool IsNonAuthenticatedRoute(string route)
        {
            return _routesConfiguration.NonAuthenticationRoutes
                .Any(r => route.Contains(r.Path!));
        }

        private void UpdateSessionCookie(SessionId sessionId, DateTime expirationDate)
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
