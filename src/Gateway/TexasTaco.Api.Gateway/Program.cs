using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TexasTaco.Api.Gateway.Authentication;

var builder = WebApplication.CreateBuilder(args);

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

        await new TexasTacoAuthenticationMiddleware(configuration)
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
