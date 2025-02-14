using Asp.Versioning;
using Microsoft.Extensions.Options;
using TexasTaco.Authentication.Api.BackgroundServices;
using TexasTaco.Authentication.Api.Configuration;
using TexasTaco.Shared.Authentication;

namespace TexasTaco.Authentication.Api
{
    public static class Extensions
    {
        internal static IServiceCollection AddTexasTacoAuthenticationApiVersioning(
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

        internal static IServiceCollection AddAuthenticationApiAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<SessionConfiguration>(
                configuration.GetRequiredSection("Session"));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<SessionConfiguration>>().Value);

            services.AddSharedAuthentication(configuration);

            return services;
        }

        internal static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<EmailNotificationsBackgroundService>();
            services.AddHostedService<AccountCreatedOutboxBackgroundService>();
            services.AddHostedService<AccountDeletedOutboxBackgroundService>();
            services.AddHostedService<DatabaseCleaningBackgroundService>();

            return services;
        }
    }
}
