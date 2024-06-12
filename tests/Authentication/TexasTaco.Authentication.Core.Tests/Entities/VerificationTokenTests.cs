using FluentAssertions;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Tests.Entities
{
    public class VerificationTokenTests
    {
        [Fact]
        public void ExpiredEarlierThan_Should_ReturnTrueIfExpiredEarlierThanSpecifiedByExpirationTimeSpan()
        {
            //Arrange
            var expirationDate = DateTime.UtcNow.AddDays(-10);

            var token = new VerificationToken(
                new AccountId(Guid.NewGuid()),
                expirationDate);

            //Act / Assert
            token.ExpiredEarlierThan(TimeSpan.FromDays(9))
                .Should()
                .BeTrue();
        }

        [Fact]
        public void ExpiredEarlierThan_Should_ReturnFalseIfNotExpiredEarlierThanSpecifiedByExpirationTimeSpan()
        {
            //Arrange
            var expirationDate = DateTime.UtcNow.AddDays(-10);

            var token = new VerificationToken(
                new AccountId(Guid.NewGuid()),
                expirationDate);

            //Act / Assert
            token.ExpiredEarlierThan(TimeSpan.FromDays(11))
                .Should()
                .BeFalse();
        }
    }
}
