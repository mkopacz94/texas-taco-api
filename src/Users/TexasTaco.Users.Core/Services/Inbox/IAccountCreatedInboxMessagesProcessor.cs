using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Services.Inbox
{
    public interface IAccountCreatedInboxMessagesProcessor
    {
        Task ProcessMessages();
    }
}
