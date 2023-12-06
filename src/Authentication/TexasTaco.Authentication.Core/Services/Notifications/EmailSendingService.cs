using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Services.Notifications
{
    internal class EmailSendingService : IEmailSendingService
    {
        public void Send(EmailNotification notificationData)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Texas Taco", "texastaco.notifications@gmail.com"));
            email.To.Add(new MailboxAddress("Mateusz Kopacz", "m.kopacz94@gmail.com"));

            email.Subject = "Confirm your email";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = "Texas Taco test email confirmation message."
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, false);

            smtp.Authenticate("Texas-Taco API", "tyol gpbo jttp fxhv");

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
