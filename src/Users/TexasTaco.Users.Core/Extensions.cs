using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TexasTaco.Shared.Settings;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.EventBus.Consumers;
using TexasTaco.Users.Core.Repositories;
using TexasTaco.Users.Core.Services.Inbox;
using TexasTaco.Users.Core.Services.Outbox;

namespace TexasTaco.Users.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddTexasTacoUsers(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddPresistence(configuration);

            services.AddScoped<IAccountCreatedInboxMessagesProcessor,
                AccountCreatedInboxMessagesProcessor>();
            services.AddScoped<IUserUpdatedOutboxMessagesProcessor,
                UserUpdatedOutboxMessagesProcessor>();

            services.Configure<MessageBrokerSettings>(
                configuration.GetSection("MessageBroker"));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddMassTransit(busConfig =>
            {
                busConfig.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(false));
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

                    config.ReceiveEndpoint("users.account-created-event-message", cfg =>
                    {
                        cfg.ConfigureConsumer<AccountCreatedEventMessageConsumer>(context);
                    });
                });
            });

            return services;
        }

        private static IServiceCollection AddPresistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<UsersDbContext>(options =>
            {
                string connectionString = configuration
                    .GetRequiredSection("UsersDatabase:ConnectionString").Value!;

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IAccountCreatedInboxMessagesRepository,
                AccountCreatedInboxMessagesRepository>();
            services.AddScoped<IPointsCollectedInboxMessagesRepository,
                PointsCollectedInboxMessagesRepository>();
            services.AddScoped<IUserUpdatedOutboxMessagesRepository,
                UserUpdatedOutboxMessagesRepository>();

            return services;
        }

        public static void ApplyDatabaseMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
