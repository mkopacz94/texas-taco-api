using FluentAssertions;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Tests.Entities
{
    public class AccountCreatedOutboxTests
    {
        [Fact]
        public void MarkAsPublished_Should_SetPublishedToCurrentDate()
        {
            //Arrange
            var accountCreatedOutbox = new AccountCreatedOutbox(
                new AccountId(Guid.NewGuid()),
                new EmailAddress("test@email.com"));

            //Act
            accountCreatedOutbox.MarkAsPublished();

            //Assert
            accountCreatedOutbox.Published
                .Should()
                .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}
