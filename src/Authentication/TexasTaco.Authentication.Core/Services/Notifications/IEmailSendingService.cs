using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Services.Notifications
{
    public interface IEmailSendingService
    {
        void Send(EmailNotification notificationData);
    }
}
