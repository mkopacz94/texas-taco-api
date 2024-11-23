using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Application.AccountCreatedInbox;
using TexasTaco.Orders.Application.Baskets;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.UnitOfWork;
using TexasTaco.Orders.Infrastructure.Data;
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            AddRepositories(services);

            return services;
        }

        public static void ApplyDatabaseMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            dbContext.Database.Migrate();
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAccountCreatedInboxMessagesRepository,
                AccountCreatedInboxMessagesRepository>();

            return services;
        }
    }
}
