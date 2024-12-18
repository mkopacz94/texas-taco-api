﻿using Ocelot.Middleware;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Model;
using TexasTaco.Api.Gateway.Routes;
using TexasTaco.Api.Gateway.Services;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Api.Gateway.Middlewares
{
    internal sealed class TexasTacoAuthenticationMiddleware(
        IAuthenticationClient authService,
        ICurrentSessionStorage currentSessionStorage,
        RoutesConfiguration routesConfiguration)
    {
        private readonly IAuthenticationClient _authClient = authService;
        private readonly ICurrentSessionStorage _currentSessionStorage = currentSessionStorage;
        private readonly RoutesConfiguration _routesConfiguration = routesConfiguration;

        public async Task InvokeAsync(HttpContext context, Func<Task> next)
        {
            string requestPath = context.Request.Path.Value!;

            if(IsNonAuthenticatedRoute(requestPath))
            {
                await next();
                return;
            }

            string? accountId = context
                .Request
                .Cookies[CookiesNames.AccountId];

            string? sessionId = context
                .Request
                .Cookies[CookiesNames.SessionId];

            var session = await _authClient.GetSession(accountId, sessionId);

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
                .Any(routeTemplate => RouteTemplateMatcher
                    .RouteEndMatchesTemplate(route, routeTemplate.Path));
        }
    }
}
