using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Net;
using TexasTaco.Authentication.Api.BackgroundServices;
using TexasTaco.Authentication.Api.Configuration;
using TexasTaco.Authentication.Api.ErrorHandling;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core;
using TexasTaco.Shared;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<EmailNotificationsBackgroundService>();
builder.Services.AddHostedService<AccountCreatedOutboxBackgroundService>();

builder.Services.AddTexasTacoAuthentication(builder.Configuration); 
builder.Services.AddSharedFramework();

builder.Services
    .AddTransient<IClaimsService, CookieClaimsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ExceptionMiddleware>();

builder.Services.Configure<SessionConfiguration>(
    builder.Configuration.GetRequiredSection("Session"));

builder.Services.AddSingleton(sp => 
    sp.GetRequiredService<IOptions<SessionConfiguration>>().Value);

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

var app = builder.Build();

app.Services.ApplyDatabaseMigrations();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
