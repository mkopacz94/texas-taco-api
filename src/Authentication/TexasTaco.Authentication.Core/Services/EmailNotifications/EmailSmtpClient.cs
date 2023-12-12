using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Services.EmailNotifications
{
    internal class EmailSmtpClient(
        ILogger<EmailSmtpClient> _logger, 
        IOptions<SmtpOptions> options) : IEmailSmtpClient
    {
        private readonly SmtpOptions _smtpOptions = options.Value;

        public async Task SendAsync(EmailNotification notificationData)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Texas Taco", _smtpOptions.SourceAddress));
            email.To.Add(new MailboxAddress(notificationData.To.Value, notificationData.To.Value));
            email.Subject = notificationData.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = notificationData.Body
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.UseSsl);
            await smtp.AuthenticateAsync(_smtpOptions.SourceAddress, _smtpOptions.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email message successfully sent to {recipient}.", notificationData.To.Value);
        }
    }
}
