using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
        | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Add(IPAddress.Parse("20.215.66.56"));
    options.ForwardLimit = 1;
});

var app = builder.Build();

app.UseForwardedHeaders();

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