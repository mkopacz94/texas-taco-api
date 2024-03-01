using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TexasTaco.Api.Gateway.Authentication;
using TexasTaco.Api.Gateway.Clients;
using TexasTaco.Api.Gateway.Model;
using TexasTaco.Api.Gateway.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddTransient<ICookieService, CookieService>();

builder.Services.AddOptions<AuthenticationHttpClientOptions>()
    .Bind(builder.Configuration.GetSection("AuthenticationHttpClient"));

builder.Services.AddHttpClient<AuthenticationClient>((serviceProvider, client) =>
{
    var authClientOptions = serviceProvider
        .GetRequiredService<IOptions<AuthenticationHttpClientOptions>>().Value;

    client.BaseAddress = new Uri(authClientOptions.BaseAddress!);
});

builder.Configuration.AddJsonFile("configuration.json");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();

var ocelotConfig = new OcelotPipelineConfiguration
{
    AuthenticationMiddleware = async (context, next) =>
    {
        var configuration = context.RequestServices
            .GetRequiredService<IConfiguration>();

        var authHttpClient = context.RequestServices
            .GetRequiredService<AuthenticationClient>();

        var cookieService = context.RequestServices
            .GetRequiredService<ICookieService>();

        await new TexasTacoAuthenticationMiddleware(
                configuration, cookieService, authHttpClient)
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
