using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace TexasTaco.Shared.Authentication
{
    public static class Extensions
    {
        public static IServiceCollection AddSharedDataProtectionCache(
            this IServiceCollection services,
           IConfiguration configuration)
        {
            string dataProtectionCacheUri = configuration
                .GetRequiredSection("DataProtectionSettings:CacheUri").Value!;

            var redis = ConnectionMultiplexer.Connect(dataProtectionCacheUri);
            services.AddDataProtection()
                .SetApplicationName(ApplicationName.Name)
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            return services;
        }
    }
}
