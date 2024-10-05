using Microsoft.Extensions.DependencyInjection;

namespace TexasTaco.Orders.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddOrdersApplication(
            this IServiceCollection services)
        {
            return services;
        }
    }
}
