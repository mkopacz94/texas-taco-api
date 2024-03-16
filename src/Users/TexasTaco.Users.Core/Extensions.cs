using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TexasTaco.Shared.Settings;
using TexasTaco.Users.Core.EventBus.Consumers;

namespace TexasTaco.Users.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddTexasTacoUsers(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MessageBrokerSettings>(
                configuration.GetSection("MessageBroker"));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddMassTransit(busConfig =>
            {
                busConfig.AddConsumer<AccountCreatedEventMessageConsumer>();

                busConfig.UsingRabbitMq((context, config) =>
                {
                    var settings = context.GetRequiredService<MessageBrokerSettings>();

                    config.Host(new Uri(settings.Host), hostConfig =>
                    {
                        hostConfig.Username(settings.Username);
                        hostConfig.Password(settings.Password);
                    });

                    config.UseMessageRetry(r => r.Interval(10, TimeSpan.FromMinutes(1)));    
                    config.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
