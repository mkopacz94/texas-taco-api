using MassTransit;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Users.Core.EventBus.Consumers
{
    internal class AccountCreatedEventMessageConsumer : IConsumer<AccountCreatedEventMessage>
    {
        public Task Consume(ConsumeContext<AccountCreatedEventMessage> context)
        {
            Console.WriteLine($"Retry count {context.GetRetryCount()}.");
            Console.WriteLine($"Redelivery count {context.GetRedeliveryCount()}.");

            throw new Exception("Consumer failed to process message.");
            return Task.CompletedTask;
        }
    }
}
