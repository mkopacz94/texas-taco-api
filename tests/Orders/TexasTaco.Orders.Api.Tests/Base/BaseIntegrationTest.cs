using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Api.Tests.Factories;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Api.Tests.Base
{
    public abstract class BaseIntegrationTest
        : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
    {
        private readonly IServiceScope _scope;
        private protected readonly OrdersDbContext DbContext;
        protected HttpClient HttpClient = default!;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();

            DbContext = _scope
                .ServiceProvider
                .GetRequiredService<OrdersDbContext>();

            DbContext.Database.Migrate();

            HttpClient = factory.CreateClient();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            DbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
