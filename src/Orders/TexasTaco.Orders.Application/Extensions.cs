using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Application.AccountCreatedInbox;
using TexasTaco.Orders.Application.Baskets;
using TexasTaco.Orders.Application.UserUpdatedInbox;

namespace TexasTaco.Orders.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddOrdersApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly);
            });

            services.AddScoped<IAccountCreatedInboxMessagesProcessor,
                AccountCreatedInboxMessagesProcessor>();
            services.AddScoped<IUserUpdatedInboxMessagesProcessor,
                UserUpdatedInboxMessagesProcessor>();

            services.AddApplicationServices();

            return services;
        }

        private static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IBasketService, BasketService>();

            return services;
        }
    }
}
