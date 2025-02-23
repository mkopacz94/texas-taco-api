using Bogus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TexasTaco.Authentication.Core.Data;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Authentication.Core.Services.EmailNotifications;
using TexasTaco.Authentication.Core.Services.Outbox;
using TexasTaco.Authentication.Core.Services.Verification;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;
using TexasTaco.Shared.Settings;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddTexasTacoAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                string connectionString = configuration
                    .GetRequiredSection("AuthenticationDatabase:ConnectionString").Value!;

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped<DbContext, AuthDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddRepositories();

            services.AddTransient<ISessionStorage, SessionStorage>();
            services.AddTransient<IPasswordManager, PasswordManager>();

            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddScoped<IAccountCreatedOutboxService, AccountCreatedOutboxService>();
            services.AddScoped<IAccountDeletedOutboxService, AccountDeletedOutboxService>();
            services.AddSmtpClient(options =>
            {
                var notificationsSettings = new EmailNotificationsSettings();
                configuration.Bind(notificationsSettings);

                options.SourceAddress = notificationsSettings.SmtpOptions!.SourceAddress!;
                options.Host = notificationsSettings.SmtpOptions!.Host!;
                options.Password = notificationsSettings.SmtpOptions!.Password!;
                options.Port = notificationsSettings.SmtpOptions!.Port!;
                options.UseSsl = notificationsSettings.SmtpOptions!.UseSsl!;
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("CacheSettings:ConnectionString").Value;
                options.InstanceName = "SessionCache";
            });

            services.Configure<MessageBrokerSettings>(
                configuration.GetSection("MessageBroker"));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddMassTransit(busConfig =>
            {
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

            var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            dbContext.Database.Migrate();
        }

        public static IServiceCollection AddSmtpClient(
            this IServiceCollection services,
            Action<SmtpOptions> configureOptions)
        {
            services.AddOptions();
            services.AddSingleton<IConfigureOptions<SmtpOptions>>(
                new ConfigureNamedOptions<SmtpOptions>(Options.DefaultName, configureOptions));
            services.AddSingleton<IEmailSmtpClient, EmailSmtpClient>();

            return services;
        }

        public static async Task SeedDatabase(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

            if (dbContext.Accounts.Any())
            {
                return;
            }

            var passwordManager = scope
                .ServiceProvider
                .GetRequiredService<IPasswordManager>();

            passwordManager
                 .HashPassword("1234", out byte[] hash, out byte[] salt);

            var fakerAccounts = new Faker<Account>()
                .CustomInstantiator(f => new Account(
                    new EmailAddress(f.Internet.Email()),
                    f.PickRandom<Role>(),
                    hash,
                    salt));

            var accounts = fakerAccounts.Generate(1000);

            var accountsOutbox = accounts
                .Select(a => new AccountCreatedOutbox(a.Id, a.Email));

            await dbContext.AddRangeAsync(accounts);
            await dbContext.AddRangeAsync(accountsOutbox);
            await dbContext.SaveChangesAsync();
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IEmailNotificationsRepository, EmailNotificationsRepository>();
            services.AddScoped<IVerificationTokensRepository, VerificationTokensRepository>();
            services.AddScoped<IAccountCreatedOutboxRepository, AccountCreatedOutboxRepository>();

            services.AddScoped<IOutboxMessagesRepository<OutboxMessage<AccountDeletedEventMessage>>,
                OutboxMessagesRepository<OutboxMessage<AccountDeletedEventMessage>>>();

            return services;
        }
    }
}
