﻿using Ocelot.Middleware;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Model;

namespace TexasTaco.Api.Gateway.Authentication
{
    internal sealed class TexasTacoAuthenticationMiddleware
    {
        private readonly AuthenticationClient _authService;
        private readonly RoutesConfiguration _routesConfiguration = new();

        public TexasTacoAuthenticationMiddleware(
            IConfiguration configuration,
            AuthenticationClient authService)
        {
            _authService = authService;
            configuration.Bind(_routesConfiguration);
        }

        public async Task InvokeAsync(HttpContext context, Func<Task> next)
        {
            string requestPath = context.Request.Path.Value!;

            if (_routesConfiguration.NonAuthenticationRoutes
                .Any(r => requestPath.Contains(r.Path!))
                || await _authService.UserSessionValid())
            {
                await next();
                return;
            }

            context.Items.SetError(new UnauthenticatedError("Authentication failed"));
        }
    }
}
