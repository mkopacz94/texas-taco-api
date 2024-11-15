using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Shared.Settings;

namespace TexasTaco.Products.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddTexasTacoProducts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ProductsDbContext>(options =>
            {
                string connectionString = configuration
                    .GetRequiredSection("ProductsDatabase:ConnectionString").Value!;

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped<IPicturesRepository, PicturesRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IPrizesRepository, PrizesRepository>();

            services.Configure<MessageBrokerSettings>(
                configuration.GetSection("MessageBroker"));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddMassTransit(busConfig =>
            {
                busConfig.SetKebabCaseEndpointNameFormatter();

                busConfig.UsingRabbitMq((context, config) =>
                {
                    var settings = context.GetRequiredService<MessageBrokerSettings>();

                    config.Host(new Uri(settings.Host), hostConfig =>
                    {
                        hostConfig.Username(settings.Username);
                        hostConfig.Password(settings.Password);
                    });
                });
            });

            return services;
        }

        public static void ApplyDatabaseMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
