namespace TexasTaco.Users.Core.Services.Outbox
{
    public interface IUserUpdatedOutboxMessagesProcessor
    {
        Task ProcessMessages();
    }
}
