namespace TexasTaco.Shared.EventBus.Users
{
    public sealed record UserUpdatedEventMessage(
        Guid AccountId,
        string FirstName,
        string LastName,
        string AddressLine,
        string PostalCode,
        string City,
        string Country);
}
