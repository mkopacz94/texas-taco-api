using MassTransit;
using TexasTaco.Shared.EventBus.Account;

namespace TexasTaco.Users.Core.EventBus.Consumers
{
    internal class AccountCreatedEventMessageConsumer : IConsumer<AccountCreatedEventMessage>
    {
        public Task Consume(ConsumeContext<AccountCreatedEventMessage> context)
        {
            Console.WriteLine($"Event consumed {context.Message.Email}.");
            return Task.CompletedTask;
        }
    }
}
