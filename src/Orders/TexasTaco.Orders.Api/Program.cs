using TexasTaco.Orders.Api;
using TexasTaco.Shared.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTexasTacoOrdersApiVersioning();
builder.Services.AddTexasTacoOrders();
builder.Services.AddOrdersOptions(builder.Configuration);
builder.Services.AddSharedDataProtectionCache(builder.Configuration);
builder.Services.AddSharedAuthentication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
