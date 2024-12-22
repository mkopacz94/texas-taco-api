using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Shared.EventBus.Account
{
    public sealed record AccountCreatedEventMessage(
        Guid Id, Guid AccountId, EmailAddress Email, DateTime PublishedDate)
        : IInboxMessageBody;
}
