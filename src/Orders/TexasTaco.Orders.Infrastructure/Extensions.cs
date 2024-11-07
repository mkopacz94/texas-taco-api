using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Application.Baskets;
using TexasTaco.Orders.Infrastructure.Data.EF;
using TexasTaco.Orders.Infrastructure.Data.Repositories;
using TexasTaco.Orders.Infrastructure.MessageBus;

namespace TexasTaco.Orders.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddOrdersInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus();

            services.AddDbContext<OrdersDbContext>(options =>
            {
                string connectionString = configuration
                    .GetRequiredSection("OrdersDatabase:ConnectionString").Value!;

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            AddRepositories(services);

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();

            return services;
        }
    }
}
