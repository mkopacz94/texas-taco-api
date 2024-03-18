using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Shared.EventBus.Account
{
    public record AccountCreatedEventMessage(
        Guid Id, Guid AccountId, EmailAddress Email, DateTime PublishedDate);
}
