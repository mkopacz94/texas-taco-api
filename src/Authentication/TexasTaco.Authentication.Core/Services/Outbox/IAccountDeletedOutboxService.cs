using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Authentication.Core.Services.Outbox
{
    public interface IAccountDeletedOutboxService
    {
        Task PublishOutboxMessage(OutboxMessage<AccountDeletedEventMessage> message);
    }
}
