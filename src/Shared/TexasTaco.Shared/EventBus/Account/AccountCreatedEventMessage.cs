using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Shared.EventBus.Account
{
    public record AccountCreatedEventMessage(
        Guid Id, EmailAddress Email, DateTime PublishedDate);
}
