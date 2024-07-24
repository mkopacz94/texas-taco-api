﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using System.Net;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Users.Api
{
    public static class Extensions
    {
        internal static IServiceCollection AddTexasTacoUsersApiVersioning(
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

        internal static IServiceCollection AddDataProtectionCache(
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

        internal static IServiceCollection AddTexasTacoUsersAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
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
    }
}
