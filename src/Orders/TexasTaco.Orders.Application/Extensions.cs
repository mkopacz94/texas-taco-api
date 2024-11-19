using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
