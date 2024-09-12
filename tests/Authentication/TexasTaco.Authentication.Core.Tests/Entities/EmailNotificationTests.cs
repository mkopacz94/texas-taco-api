using FluentAssertions;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Tests.Entities
{
    public class EmailNotificationTests
    {
        [Fact]
        public void MarkAsSent_Should_SetStatusToSent()
        {
            //Arrange
            var emailNotification = new EmailNotification(
                "subject",
                "body",
                new EmailAddress("test@email.com"),
                new EmailAddress("test@email.com"));

            //Act
            emailNotification.MarkAsSent();

            //Assert
            emailNotification.Status
                .Should()
                .Be(EmailNotificationStatus.Sent);
        }
    }
}
