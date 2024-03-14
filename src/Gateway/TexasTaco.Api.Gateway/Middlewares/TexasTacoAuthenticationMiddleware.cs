using Ocelot.Middleware;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Model;
using TexasTaco.Api.Gateway.Services;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Api.Gateway.Middlewares
{
    internal sealed class TexasTacoAuthenticationMiddleware
    {
        private readonly IAuthenticationClient _authClient;
        private readonly ICurrentSessionStorage _currentSessionStorage;
        private readonly RoutesConfiguration _routesConfiguration;

        public TexasTacoAuthenticationMiddleware(
            IAuthenticationClient authService,
            ICurrentSessionStorage currentSessionStorage,
            RoutesConfiguration routesConfiguration)
        {
            _authClient = authService;
            _currentSessionStorage = currentSessionStorage;
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
                _currentSessionStorage.SaveSessionInStorage(session);

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
    }
}
