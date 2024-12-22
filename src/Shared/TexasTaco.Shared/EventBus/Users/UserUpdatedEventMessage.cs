using TexasTaco.Shared.Inbox;

namespace TexasTaco.Shared.EventBus.Users
{
    public sealed record UserUpdatedEventMessage(
        Guid Id,
        Guid AccountId,
        string FirstName,
        string LastName,
        string AddressLine,
        string PostalCode,
        string City,
        string Country)
        : IInboxMessageBody;
}
