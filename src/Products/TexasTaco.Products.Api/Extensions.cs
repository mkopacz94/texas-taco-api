using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Reflection;
using TexasTaco.Products.Api.Endpoints;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Products.Api
{
    public static class Extensions
    {
        internal static IServiceCollection AddTexasTacoProductsAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddAuthorization()
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x =>
                {
                    string cookieDomain = configuration.GetRequiredSection("AuthCookies:Domain").Value!;
                    int expirationMinutes = int.Parse(
                        configuration.GetRequiredSection("AuthCookies:ExpirationMinutes").Value!);

                    x.Cookie.Name = CookiesNames.ApiClaims;
                    x.Cookie.HttpOnly = true;
                    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    x.Cookie.SameSite = SameSiteMode.Strict;
                    x.Cookie.Domain = cookieDomain;

                    x.ExpireTimeSpan = TimeSpan.FromMinutes(expirationMinutes);
                    x.SlidingExpiration = true;

                    x.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                    x.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    };
                });

            return services;
        }

        internal static IServiceCollection AddTexasTacoProductsApiVersioning(
            this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        internal static IServiceCollection AddEndpoints(
            this IServiceCollection services,
            Assembly assembly)
        {
            var serviceDescriptors = assembly
                .DefinedTypes
                .Where(t => t is { IsAbstract: false, IsInterface: false }
                    && t.IsAssignableTo(typeof(IEndpoint)))
                .Select(t => ServiceDescriptor.Transient(typeof(IEndpoint), t))
                .ToArray();

            services.TryAddEnumerable(serviceDescriptors);

            return services;
        }

        internal static IApplicationBuilder MapEndpoints(
            this WebApplication app,
            RouteGroupBuilder? routeGroupBuilder = null)
        {
            var endpoints = app.Services
                .GetRequiredService<IEnumerable<IEndpoint>>();

            IEndpointRouteBuilder endpointRouteBuilder =
                routeGroupBuilder is null ? app : routeGroupBuilder;

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(endpointRouteBuilder);
            }

            return app;
        }
    }
}
