using Microsoft.Extensions.DependencyInjection;

namespace TexasTaco.Orders.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddOrdersDomain(
            this IServiceCollection services)
        {
            return services;
        }
    }
}
