using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Shared.Services;

namespace TexasTaco.Shared
{
    public static class Extensions
    {
        public static IServiceCollection AddSharedFramework(this IServiceCollection services)
        {
            services.AddCookieService();
            return services;
        }
    }
}
