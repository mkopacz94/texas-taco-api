using Microsoft.Extensions.DependencyInjection;

namespace TexasTaco.Shared.Services
{
    internal static class Extensions
    {
        public static IServiceCollection AddCookieService(this IServiceCollection services) 
        {
            services.AddScoped<ICookieService, CookieService>();
            return services;
        }
    }
}
