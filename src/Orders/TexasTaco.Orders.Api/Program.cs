using System.Text.Json.Serialization;
using TexasTaco.Orders.Api;
using TexasTaco.Orders.Api.ErrorHandling;
using TexasTaco.Orders.Infrastructure;
using TexasTaco.Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddTexasTacoOrdersApiVersioning();
builder.Services.AddTexasTacoOrders(builder.Configuration);
builder.Services.AddOrdersOptions(builder.Configuration);
builder.Services.AddSharedDataProtectionCache(builder.Configuration);
builder.Services.AddSharedAuthentication(builder.Configuration);
builder.Services.AddOrdersHostedServices();
builder.Services.AddSingleton<ExceptionMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Services.ApplyDatabaseMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
