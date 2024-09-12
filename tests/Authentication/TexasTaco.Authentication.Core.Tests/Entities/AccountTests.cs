using FluentAssertions;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Tests.Entities
{
    public class AccountTests
    {
        [Fact]
        public void MarkAsVerified_Should_SetVerifiedToTrue()
        {
            //Arrange
            var account = new Account(
                new EmailAddress("test@email.com"),
                Role.Customer,
                [],
                []);

            //Act
            account.MarkAsVerified();

            //Assert
            account.Verified
                .Should()
                .BeTrue();
        }
    }
}
