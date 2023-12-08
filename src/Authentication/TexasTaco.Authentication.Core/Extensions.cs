﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services;
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
    }
}
