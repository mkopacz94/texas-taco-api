using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net;
using TexasTaco.Api.Gateway.Authentication;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Model;
using TexasTaco.Api.Gateway.Services;
using TexasTaco.Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("configuration.json");

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

builder.Services
    .AddTransient<ICookieService, CookieService>();

builder.Services.AddOptions<AuthenticationHttpClientOptions>()
    .Bind(builder.Configuration.GetSection("AuthenticationHttpClient"));

builder.Services.Configure<RoutesConfiguration>(
    builder.Configuration.GetRequiredSection("NonAuthRoutesConfiguration"));

builder.Services.AddSingleton(sp => 
    sp.GetRequiredService<IOptions<RoutesConfiguration>>().Value);

builder.Services.AddHttpClient<IAuthenticationClient, AuthenticationClient>(
    (serviceProvider, client) =>
    {
        var authClientOptions = serviceProvider
            .GetRequiredService<IOptions<AuthenticationHttpClientOptions>>().Value;

        client.BaseAddress = new Uri(authClientOptions.BaseAddress!);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();

var ocelotConfig = new OcelotPipelineConfiguration
{
    AuthenticationMiddleware = async (context, next) =>
    {
        var routesConfiguration = context.RequestServices
            .GetRequiredService<RoutesConfiguration>();

        var authHttpClient = context.RequestServices
            .GetRequiredService<IAuthenticationClient>();

        var cookieService = context.RequestServices
            .GetRequiredService<ICookieService>();

        await new TexasTacoAuthenticationMiddleware(
                cookieService, authHttpClient, routesConfiguration)
            .InvokeAsync(context, next);
    }
};

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseOcelot(ocelotConfig);
app.Run();
