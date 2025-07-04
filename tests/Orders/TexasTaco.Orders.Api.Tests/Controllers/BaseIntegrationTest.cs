using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Api.Tests.Factories;
using TexasTaco.Orders.Infrastructure.Data.EF;

namespace TexasTaco.Orders.Api.Tests.Controllers
{
    public abstract class BaseIntegrationTest
        : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
    {
        private readonly IServiceScope _scope;
        private protected readonly OrdersDbContext DbContext;
        protected HttpClient Client = default!;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();

            DbContext = _scope
                .ServiceProvider
                .GetRequiredService<OrdersDbContext>();

            DbContext.Database.Migrate();

            Client = factory.CreateClient();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            DbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
