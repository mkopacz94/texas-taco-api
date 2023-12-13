using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.Repositories;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Services.Verification
{
    internal class EmailVerificationService(
        IVerificationTokensRepository _tokensRepository,
        IEmailNotificationsRepository _emailNotificationsRepository) : IEmailVerificationService
    {
        public async Task EnqueueVerificationEmail(Account account)
        {
            var verificationToken = new VerificationToken(
                account.Id, DateTime.UtcNow.AddHours(1));

            await _tokensRepository.AddAsync(verificationToken);

            var emailNotification = new EmailNotification(
                "Texas Taco account verification",
                CreateVerificationEmailBody(),
                new EmailAddress("texastaco.notifications@gmail.com"),
                account.Email);

            await _emailNotificationsRepository.AddAsync(emailNotification);
        }

        private static string CreateVerificationEmailBody()
        {
            return @"
                <div style='font-family: 'Helvetica';'>
                    <p style='font-size: 1.5em;'>Texas Taco</p>
                    <p>Hi,</p>
                    <p>Thank you for signing up for <span style='font-weight: bold;'>Texas Taco</span>. To activate your account, please verify your email address.</p>
                    <a href='https://www.google.com/'><button style='padding: 0.8em 1.5em; border-radius: 50px; border: 0px; color: white; cursor: pointer; background: #ffa436;'>Verify Now</button></a>
                    <p style='font-size: 0.8em; margin-top: 3em;'>Ignore this email if you received it without signing up.</p>
                </div>";
        }
    }
}
