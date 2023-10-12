using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor 
        | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/orders", (HttpContext context) =>
{
    var stringBuilder = new StringBuilder();
    stringBuilder.AppendLine($"Remote IP address: {context.Connection.RemoteIpAddress}");
    stringBuilder.AppendLine($"X-Forwared-For: {context.Request.Headers["X-Forwarded-For"]}");

    return stringBuilder.ToString();
})
.WithName("GetOrders");

app.Run();