using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Clients.Handlers;
using TexasTaco.Api.Gateway.Configuration;
using TexasTaco.Api.Gateway.Middlewares;
using TexasTaco.Api.Gateway.Model;
using TexasTaco.Api.Gateway.Services;
using TexasTaco.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("configuration.json");

builder.Services.AddOptions<AuthenticationHttpClientOptions>()
    .Bind(builder.Configuration.GetSection("AuthenticationHttpClient"));

builder.Services.Configure<RoutesConfiguration>(
    builder.Configuration.GetRequiredSection("NonAuthRoutesConfiguration"));

builder.Services.AddOptions<ApplicationConfiguration>()
    .Bind(builder.Configuration.GetSection(nameof(ApplicationConfiguration)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<RoutesConfiguration>>().Value);

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<ApplicationConfiguration>>().Value);

builder.Services.AddSharedFramework();
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

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AdminClientCorsPolicy",
        policy =>
        {
            var corsSettings = builder
                .Configuration
                .GetSection($"{nameof(ApplicationConfiguration)}" +
                    $":{nameof(ApplicationConfiguration.Cors)}");

            var allowedOrigins = corsSettings
                .GetSection(nameof(ApplicationConfiguration.Cors.AllowedOrigins))
                .Get<string[]>() ?? [];

            var allowedMethods = corsSettings
                .GetSection(nameof(ApplicationConfiguration.Cors.AllowedMethods))
                .Get<string[]>() ?? [];

            var allowedHeaders = corsSettings
                .GetSection(nameof(ApplicationConfiguration.Cors.AllowedHeaders))
                .Get<string[]>() ?? [];

            policy
                .WithOrigins(allowedOrigins)
                .WithHeaders(allowedHeaders)
                .WithMethods(allowedMethods)
                .AllowCredentials();
        });
});

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

app.UseCors("AdminClientCorsPolicy");
await app.UseOcelot(ocelotConfig);
app.Run();
