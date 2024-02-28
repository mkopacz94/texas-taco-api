﻿using MassTransit;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.Services.Outbox;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Authentication.Api.BackgroundServices
{
    public class AccountCreatedOutboxBackgroundService(
        ILogger<AccountCreatedOutboxBackgroundService> _logger,
        IServiceProvider _serviceProvider)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var authenticationRepository = scope
                    .ServiceProvider
                    .GetRequiredService<IAuthenticationRepository>();

                var outboxService = scope
                    .ServiceProvider
                    .GetRequiredService<IAccountCreatedOutboxService>();

                var messagesToBePublished = await authenticationRepository
                    .GetNonPublishedUserCreatedOutboxMessages();

                foreach (var message in messagesToBePublished)
                {
                    try
                    {
                        _logger.LogInformation("Processing outbox message with Id={messageId}...", message.Id);

                        await outboxService.PublishAccountCreatedOutboxMessage(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex, 
                            "Error occured during processing outbox message with Id={messageId}.", 
                            message.Id);
                    }
                }
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}