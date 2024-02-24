using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using TexasTaco.Authentication.Api.BackgroundServices;
using TexasTaco.Authentication.Api.ErrorHandling;
using TexasTaco.Authentication.Api.Services;
using TexasTaco.Authentication.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<EmailNotificationsBackgroundService>();
builder.Services.AddHostedService<AccountCreatedOutboxBackgroundService>();
builder.Services
    .AddTexasTacoAuthentication(builder.Configuration);
builder.Services
    .AddTransient<ICookieService, CookieService>();
builder.Services
    .AddTransient<IClaimsService, CookieClaimsService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ExceptionMiddleware>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.Cookie.HttpOnly = true;
        x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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
