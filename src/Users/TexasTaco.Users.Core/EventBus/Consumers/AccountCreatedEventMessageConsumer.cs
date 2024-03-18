using MassTransit;
using Microsoft.Extensions.Logging;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.Repositories;

namespace TexasTaco.Users.Core.EventBus.Consumers
{
    internal class AccountCreatedEventMessageConsumer(
        IUsersRepository _usersRepository, ILogger<AccountCreatedEventMessageConsumer> _logger) 
        : IConsumer<AccountCreatedEventMessage>
    {
        public async Task Consume(ConsumeContext<AccountCreatedEventMessage> context)
        {
            var message = context.Message;
            var user = new User(message.AccountId, message.Email);

            try
            {
                await _usersRepository.AddUserAsync(user);

                _logger.LogInformation("Added user with accountId {accountId} and email address " +
                    "{emailAddress} to users database.", user.AccountId, user.Email.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add user with accountId {accountId} " +
                    "and email address {emailAddress} to users database", user.AccountId, user.Email.Value);
            }
        }
    }
}
