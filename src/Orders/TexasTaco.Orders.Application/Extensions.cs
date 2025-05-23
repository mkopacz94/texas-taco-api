﻿using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Orders.Application.AccountCreatedInbox;
using TexasTaco.Orders.Application.AccountDeletedInbox;
using TexasTaco.Orders.Application.ProductPriceChangedInbox;
using TexasTaco.Orders.Application.UserUpdatedInbox;

namespace TexasTaco.Orders.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddOrdersApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly);
            });

            services.AddScoped<IAccountCreatedInboxMessagesProcessor,
                AccountCreatedInboxMessagesProcessor>();
            services.AddScoped<IAccountDeletedInboxMessagesProcessor,
                AccountDeletedInboxMessagesProcessor>();
            services.AddScoped<IUserUpdatedInboxMessagesProcessor,
                UserUpdatedInboxMessagesProcessor>();
            services.AddScoped<IProductPriceChangedInboxMessagesProcessor,
                ProductPriceChangedInboxMessagesProcessor>();

            services.AddApplicationServices();

            return services;
        }

        private static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            return services;
        }
    }
}
