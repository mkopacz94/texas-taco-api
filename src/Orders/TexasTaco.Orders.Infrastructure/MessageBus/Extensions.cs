﻿using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
                busConfig.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(false));

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
