using TexasTaco.Shared.Authentication;
using TexasTaco.Users.Api;
using TexasTaco.Users.Api.OpenApi;
using TexasTaco.Users.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
builder.Services.AddTexasTacoUsersApiVersioning();
builder.Services.AddSharedDataProtectionCache(builder.Configuration);
builder.Services.AddSharedAuthentication(builder.Configuration);

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
