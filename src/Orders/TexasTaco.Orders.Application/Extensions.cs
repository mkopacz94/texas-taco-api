using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Application.AccountCreatedInbox;

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

            return services;
        }
    }
}
