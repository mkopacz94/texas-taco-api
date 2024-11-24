using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TexasTaco.Orders.Infrastructure.MessageBus.Consumers;
using TexasTaco.Shared.Settings;

namespace TexasTaco.Orders.Infrastructure.MessageBus
{
    internal static class Extensions
    {
        internal static IServiceCollection AddMessageBus(
            this IServiceCollection services)
        {
            services.AddSingleton(sp =>
               sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddMassTransit(busConfig =>
            {
                busConfig.SetKebabCaseEndpointNameFormatter();

                busConfig.AddConsumer<AddProductToBasketRequestConsumer>();
                busConfig.AddConsumer<AccountCreatedEventMessageConsumer>();
                busConfig.AddConsumer<UserUpdatedEventMessageConsumer>();

                busConfig.UsingRabbitMq((context, config) =>
                {
                    var settings = context.GetRequiredService<MessageBrokerSettings>();

                    config.Host(new Uri(settings.Host), hostConfig =>
                    {
                        hostConfig.Username(settings.Username);
                        hostConfig.Password(settings.Password);
                    });

                    config.UseMessageRetry(r => r.Interval(10, TimeSpan.FromMinutes(1)));

                    config.ReceiveEndpoint("orders.account-created-event-message", cfg =>
                    {
                        cfg.ConfigureConsumer<AccountCreatedEventMessageConsumer>(context);
                    });

                    config.ReceiveEndpoint("orders.user-updated-event-message", cfg =>
                    {
                        cfg.ConfigureConsumer<UserUpdatedEventMessageConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}
