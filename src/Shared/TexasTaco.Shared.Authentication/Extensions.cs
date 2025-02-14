using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Net;

namespace TexasTaco.Shared.Authentication
{
    public static class Extensions
    {
        public static IServiceCollection AddSharedDataProtectionCache(
            this IServiceCollection services,
           IConfiguration configuration)
        {
            string dataProtectionCacheUri = configuration
                .GetRequiredSection("DataProtectionSettings:CacheUri").Value!;

            var redis = ConnectionMultiplexer.Connect(dataProtectionCacheUri);
            services.AddDataProtection()
                .SetApplicationName(ApplicationName.Name)
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            return services;
        }

        public static IServiceCollection AddSharedAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddAuthorization()
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x =>
                {
                    string cookieDomain = configuration
                        .GetRequiredSection("AuthCookies:Domain")
                        .Value!;

                    int expirationMinutes = int.Parse(
                        configuration
                            .GetRequiredSection("AuthCookies:ExpirationMinutes")
                            .Value!);

                    x.Cookie.Name = CookiesNames.ApiClaims;
                    x.Cookie.HttpOnly = true;
                    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    x.Cookie.SameSite = SameSiteMode.None;
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
    }
}
