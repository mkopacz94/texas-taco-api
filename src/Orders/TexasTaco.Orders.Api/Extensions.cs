using Asp.Versioning;
using TexasTaco.Orders.Api.BackgroundServices;
using TexasTaco.Orders.Application;
using TexasTaco.Orders.Domain;
using TexasTaco.Orders.Infrastructure;
using TexasTaco.Shared.Settings;

namespace TexasTaco.Orders.Api
{
    public static class Extensions
    {
        internal static IServiceCollection AddTexasTacoOrders(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOrdersApplication()
                .AddOrdersDomain()
                .AddOrdersInfrastructure(configuration);

            return services;
        }

        internal static IServiceCollection AddOrdersOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MessageBrokerSettings>(
                configuration.GetSection("MessageBroker"));

            return services;
        }

        internal static IServiceCollection AddTexasTacoOrdersApiVersioning(
            this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        internal static IServiceCollection AddOrdersHostedServices(
            this IServiceCollection services)
        {
            services.AddHostedService<AccountCreatedInboxBackgroundService>();
            services.AddHostedService<UserUpdatedInboxBackgroundService>();
            services.AddHostedService<ProductPriceChangedInboxBackgroundService>();
            return services;
        }
    }
}
