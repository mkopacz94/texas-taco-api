using Asp.Versioning;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using TexasTaco.Products.Api;
using TexasTaco.Products.Api.Clients;
using TexasTaco.Products.Api.Configuration;
using TexasTaco.Products.Api.OpenApi;
using TexasTaco.Products.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
builder.Services.AddTexasTacoProductsApiVersioning();

builder.Services.AddDataProtectionCache(
    builder.Configuration);

builder.Services.AddTexasTacoProductsAuthentication(
    builder.Configuration);

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "XSRF-TOKEN";
    options.Cookie.Name = "asp-net-xsrf-cookie";
});

builder.Services.AddTexasTacoProducts(builder.Configuration);
builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddOptions<AwsS3BucketClientOptions>()
    .Bind(builder.Configuration.GetSection("AwsS3BucketClientOptions"));

builder.Services.AddHttpClient<IAwsS3BucketClient, AwsS3BucketClient>(
    (serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<AwsS3BucketClientOptions>>().Value;

    client.BaseAddress = new Uri(options.ApiBaseAddress!);
    client.DefaultRequestHeaders.Add("x-api-key", options.ApiKey);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(opt =>
{
    opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

var versionedGroup = app
    .MapGroup("api/v{version:apiVersion}/products")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

app.Services.ApplyDatabaseMigrations();

// Configure the HTTP request pipeline.
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
app.UseAntiforgery();

app.Run();