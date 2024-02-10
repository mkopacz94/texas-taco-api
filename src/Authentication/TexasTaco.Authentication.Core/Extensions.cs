using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Authentication.Core.Services.EmailNotifications;
using TexasTaco.Authentication.Core.Services.Verification;

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
            services.AddTransient<ISessionStorage, SessionStorage>();
            services.AddTransient<IPasswordManager, PasswordManager>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IEmailNotificationsRepository, EmailNotificationsRepository>();
            services.AddScoped<IVerificationTokensRepository, VerificationTokensRepository>();  
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
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
    }
}
