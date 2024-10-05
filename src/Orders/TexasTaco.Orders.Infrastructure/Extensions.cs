using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Infrastructure.MessageBus;

namespace TexasTaco.Orders.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddOrdersInfrastructure(
            this IServiceCollection services)
        {
            services.AddMessageBus();
            return services;
        }
    }
}
