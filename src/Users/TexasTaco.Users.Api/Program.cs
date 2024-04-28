using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using System.Net;
using TexasTaco.Shared.Authentication;
using TexasTaco.Users.Api;
using TexasTaco.Users.Api.OpenApi;
using TexasTaco.Users.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
builder.Services.AddTexasTacoUsersApiVersioning();

string dataProtectionCacheUri = builder.Configuration
    .GetRequiredSection("DataProtectionSettings:CacheUri").Value!;

var redis = ConnectionMultiplexer.Connect(dataProtectionCacheUri);
builder.Services.AddDataProtection()
    .SetApplicationName(ApplicationName.Name)
    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        var configuration = builder.Configuration;
        string cookieDomain = configuration.GetRequiredSection("AuthCookies:Domain").Value!;
        int expirationMinutes = int.Parse(
            configuration.GetRequiredSection("AuthCookies:ExpirationMinutes").Value!);

        x.Cookie.Name = CookiesNames.ApiClaims;
        x.Cookie.HttpOnly = true;
        x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTexasTacoUsers(builder.Configuration);

var app = builder.Build();

app.Services.ApplyDatabaseMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var apiDescriptions = app.DescribeApiVersions();

        foreach (var description in apiDescriptions)
        {
            string url = $"{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
