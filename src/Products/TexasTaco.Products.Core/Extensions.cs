using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Products.Core.Data.EF;
using TexasTaco.Products.Core.Repositories;

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
