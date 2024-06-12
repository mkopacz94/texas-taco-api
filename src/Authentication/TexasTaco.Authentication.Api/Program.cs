using TexasTaco.Authentication.Api;
using TexasTaco.Authentication.Api.BackgroundServices;
using TexasTaco.Authentication.Api.ErrorHandling;
using TexasTaco.Authentication.Api.OpenApi;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core;
using TexasTaco.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddBackgroundServices();

builder.Services.AddTexasTacoAuthenticationApiVersioning();
builder.Services.AddTexasTacoAuthentication(builder.Configuration); 
builder.Services.AddSharedFramework();

builder.Services
    .AddTransient<IClaimsService, CookieClaimsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ExceptionMiddleware>();

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

builder.Services.AddDataProtectionCache(builder.Configuration);
builder.Services.AddTexasTacoApiAuthentication(builder.Configuration);

var app = builder.Build();

app.Services.ApplyDatabaseMigrations();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
