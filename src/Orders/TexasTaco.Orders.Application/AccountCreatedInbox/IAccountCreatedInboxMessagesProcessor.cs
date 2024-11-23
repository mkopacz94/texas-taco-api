namespace TexasTaco.Orders.Application.AccountCreatedInbox
{
    public interface IAccountCreatedInboxMessagesProcessor
    {
        Task ProcessMessages();
    }
}
