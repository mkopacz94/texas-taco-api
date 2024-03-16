using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Clients.Handlers;
using TexasTaco.Api.Gateway.Middlewares;
using TexasTaco.Api.Gateway.Model;
using TexasTaco.Api.Gateway.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("configuration.json");

builder.Services.AddOptions<AuthenticationHttpClientOptions>()
    .Bind(builder.Configuration.GetSection("AuthenticationHttpClient"));

builder.Services.Configure<RoutesConfiguration>(
    builder.Configuration.GetRequiredSection("NonAuthRoutesConfiguration"));

builder.Services.AddSingleton(sp => 
    sp.GetRequiredService<IOptions<RoutesConfiguration>>().Value);

builder.Services.AddTransient<ICookieService, CookieService>();
builder.Services.AddScoped<ICurrentSessionStorage, InMemoryCurrentSessionStorage>();
builder.Services.AddTransient<ISessionCookieUpdater, SessionCookieUpdater>();
builder.Services.AddTransient<CopyCookiesHandler>();

builder.Services
    .AddHttpClient<IAuthenticationClient, AuthenticationClient>(
        (serviceProvider, client) =>
        {
            var authClientOptions = serviceProvider
                .GetRequiredService<IOptions<AuthenticationHttpClientOptions>>().Value;

            client.BaseAddress = new Uri(authClientOptions.BaseAddress!);
        })
    .AddHttpMessageHandler<CopyCookiesHandler>();

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

        var currentSessionStorage = context.RequestServices
            .GetRequiredService<ICurrentSessionStorage>();

        await new TexasTacoAuthenticationMiddleware(
                authHttpClient, currentSessionStorage, routesConfiguration)
            .InvokeAsync(context, next);
    },
    PreQueryStringBuilderMiddleware = async (context, next) =>
    {
        await next();

        var sessionCookieUpdates = context.RequestServices
            .GetRequiredService<ISessionCookieUpdater>();

        sessionCookieUpdates.UpdateSessionCookie(context);
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
