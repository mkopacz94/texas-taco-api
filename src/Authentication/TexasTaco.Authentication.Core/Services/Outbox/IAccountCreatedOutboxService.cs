using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Services.Outbox
{
    public interface IAccountCreatedOutboxService
    {
        Task PublishAccountCreatedOutboxMessage(AccountCreatedOutbox message);
    }
}
