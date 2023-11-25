using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Repositories;

namespace TexasTaco.Authentication.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddTexasTacoAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                string connectionString = configuration
                    .GetRequiredSection("AuthenticationDatabase:ConnectionString").Value!;

                Console.WriteLine(connectionString);
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            services.AddTransient<ISessionRepository, SessionRepository>();
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("CacheSettings:ConnectionString").Value;
                options.InstanceName = "AuthenticationCache";
            });

            return services;
        }

        public static void ApplyDatabaseMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
