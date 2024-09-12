using FluentAssertions;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Tests.Entities
{
    public class SessionTests
    {
        [Fact]
        public void IsBeforeHalfOfExpirationTime_Should_ReturnTrueIfBeforeHalfOfExpirationDate()
        {
            //Arrange
            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow.AddSeconds(11),
                false);

            //Act
            bool isBeforeHalfOfExpirationTime = session.IsBeforeHalfOfExpirationTime();

            //Assert
            isBeforeHalfOfExpirationTime
                .Should()
                .BeTrue();
        }

        [Fact]
        public void IsBeforeHalfOfExpirationTime_Should_ReturnFalseIfAfterHalfOfExpirationDate()
        {
            //Arrange
            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow.AddSeconds(9),
                false);

            //Act
            bool isBeforeHalfOfExpirationTime = session.IsBeforeHalfOfExpirationTime();

            //Assert
            isBeforeHalfOfExpirationTime
                .Should()
                .BeFalse();
        }

        [Fact]
        public void ExtendSession_Should_UpdateLastExtensionDateToUtcNow()
        {
            //Arrange
            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow.AddSeconds(9),
                false);

            //Act
            session.ExtendSession(TimeSpan.FromSeconds(10));

            //Assert
            session
                .LastExtensionDate
                .Should()
                .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void ExtendSession_Should_UpdateExpirationDateByGivenTimeSpan()
        {
            //Arrange
            int secondsToExtendSession = 9;

            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow,
                false);

            //Act
            session.ExtendSession(TimeSpan.FromSeconds(secondsToExtendSession));

            //Assert
            session
                .ExpirationDate
                .Should()
                .BeCloseTo(
                    DateTime.UtcNow.AddSeconds(secondsToExtendSession), 
                    TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Revoke_Should_SetRevokedToTrue()
        {
            //Arrange
            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow,
                false);

            //Act
            session.Revoke();

            //Assert
            session
                .Revoked
                .Should()
                .BeTrue();
        }

        [Fact]
        public void IsValid_Should_ReturnFalseIfSessionIsRevoked()
        {
            //Arrange
            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow,
                true);

            //Act
            bool isValid = session.IsValid();

            //Assert
            isValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public void IsValid_Should_ReturnFalseIfSessionExpired()
        {
            //Arrange
            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow.AddSeconds(-5),
                false);

            //Act
            bool isValid = session.IsValid();

            //Assert
            isValid
                .Should()
                .BeFalse();
        }

        [Fact]
        public void IsValid_Should_ReturnTrueIfSessionNotRevokedNorExpired()
        {
            //Arrange
            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow.AddSeconds(10),
                false);

            //Act
            bool isValid = session.IsValid();

            //Assert
            isValid
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Update_Should_UpdateLastExtensionDateExpirationDateAndRevoked()
        {
            //Arrange
            var session = new Session(
                new SessionId(Guid.NewGuid()),
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow.AddSeconds(10),
                false);

            var updatedLastExtensionDate = DateTime.UtcNow.AddSeconds(20);
            var updatedExpirationDate = DateTime.UtcNow.AddSeconds(30);

            var newSession = new Session(
                new SessionId(Guid.NewGuid()),
                updatedLastExtensionDate,
                updatedExpirationDate,
                true);

            //Act
            session.Update(newSession);

            //Assert
            session.LastExtensionDate
                .Should()
                .BeCloseTo(updatedLastExtensionDate, TimeSpan.FromSeconds(1));

            session.ExpirationDate
                .Should()
                .BeCloseTo(updatedExpirationDate, TimeSpan.FromSeconds(1));

            session.Revoked
                .Should()
                .BeTrue();
        }
    }
}
