using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Application.Carts;
using TexasTaco.Orders.Application.Customers;
using TexasTaco.Orders.Application.Orders;
using TexasTaco.Orders.Application.PointsCollectedOutbox;
using TexasTaco.Orders.Application.Shared;
using TexasTaco.Orders.Infrastructure.Data;
using TexasTaco.Orders.Infrastructure.Data.EF;
using TexasTaco.Orders.Infrastructure.Data.Repositories;
using TexasTaco.Orders.Infrastructure.MessageBus;
using TexasTaco.Orders.Infrastructure.Outbox;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.EventBus.Products;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Inbox.Repository;

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

            services.AddScoped<DbContext, OrdersDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            AddRepositories(services);
            AddOutbox(services);

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
            services.AddScoped<ICartsRepository, CartsRepository>();
            services.AddScoped<ICartsRepository, CartsRepository>();
            services.AddScoped<ICheckoutCartsRepository, CheckoutCartsRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IInboxMessagesRepository<InboxMessage<AccountCreatedEventMessage>>,
                InboxMessagesRepository<InboxMessage<AccountCreatedEventMessage>>>();
            services.AddScoped<IInboxMessagesRepository<InboxMessage<UserUpdatedEventMessage>>,
                InboxMessagesRepository<InboxMessage<UserUpdatedEventMessage>>>();
            services.AddScoped<IInboxMessagesRepository<InboxMessage<ProductPriceChangedEventMessage>>,
                InboxMessagesRepository<InboxMessage<ProductPriceChangedEventMessage>>>();
            services.AddScoped<IPointsCollectedOutboxMessagesRepository,
                PointsCollectedOutboxMessagesRepository>();

            return services;
        }

        private static IServiceCollection AddOutbox(
            this IServiceCollection services)
        {
            services.AddScoped<IPointsCollectedOutboxMessagesProcessor,
                PointsCollectedOutboxMessagesProcessor>();

            return services;
        }
    }
}
