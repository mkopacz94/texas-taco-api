using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MySql;
using TexasTaco.Orders.Api.Tests.Auth;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Api.Tests.Factories
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MySqlContainer _dbContainer = new MySqlBuilder()
            .WithImage("mysql:8.0")
            .WithPassword("test_password")
            .WithCleanUp(true)
            .Build();

        public Task InitializeAsync()
        {
            return _dbContainer.StartAsync();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services
                .AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme",
                        options => { });

                services.AddAuthorizationBuilder()
                    .AddPolicy("YourPolicy", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole("Admin");
                    });

                services.PostConfigure<AuthenticationOptions>(opts =>
                {
                    opts.DefaultAuthenticateScheme = "TestScheme";
                    opts.DefaultChallengeScheme = "TestScheme";
                });

                var descriptorType = typeof(DbContextOptions<OrdersDbContext>);

                var descriptor = services
                    .SingleOrDefault(s => s.ServiceType == descriptorType);

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<OrdersDbContext>(options =>
                {
                    string connectionString = _dbContainer.GetConnectionString();

                    options.UseMySql(
                        connectionString,
                        ServerVersion.AutoDetect(connectionString));
                });
            });

            base.ConfigureWebHost(builder);
        }
    }
}
