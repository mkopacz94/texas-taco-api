using TexasTaco.Authentication.Api.ErrorHandling;
using TexasTaco.Authentication.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddTexasTacoAuthentication(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ExceptionMiddleware>();

var app = builder.Build();

app.Services.ApplyDatabaseMigrations();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
