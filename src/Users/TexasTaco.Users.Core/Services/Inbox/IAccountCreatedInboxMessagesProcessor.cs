using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasTaco.Users.Core.Services.Inbox
{
    public interface IAccountCreatedInboxMessagesProcessor
    {
        Task ProcessMessage();
    }
}
