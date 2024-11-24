using Asp.Versioning;
using TexasTaco.Users.Api.BackgroundServices;

namespace TexasTaco.Users.Api
{
    public static class Extensions
    {
        internal static IServiceCollection AddTexasTacoUsersApiVersioning(
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

        internal static IServiceCollection AddUsersHostedServices(
            this IServiceCollection services)
        {
            services.AddHostedService<AccountCreatedInboxBackgroundService>();
            services.AddHostedService<UserUpdatedOutboxBackgroundService>();

            return services;
        }
    }
}
