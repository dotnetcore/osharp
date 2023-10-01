using Microsoft.AspNetCore.Hosting.Server;

using OSharp.Extensions;

using Xunit;


namespace OSharp.Authorization.Tests
{
    public class AuthorizationResultTests
    {
        [Fact]
        public void IsOk_ReturnsTrue_WhenResultTypeIsOk()
        {
            // Arrange
            var result = new AuthorizationResult(AuthorizationStatus.OK);

            // Act
            var isOk = result.IsOk;

            // Assert
            Assert.True(isOk);
        }

        [Fact]
        public void IsUnauthorized_ReturnsTrue_WhenResultTypeIsUnauthorized()
        {
            // Arrange
            var result = new AuthorizationResult(AuthorizationStatus.Unauthorized);

            // Act
            var isUnauthorized = result.IsUnauthorized;

            // Assert
            Assert.True(isUnauthorized);
        }

        [Fact]
        public void IsForbidden_ReturnsTrue_WhenResultTypeIsForbidden()
        {
            // Arrange
            var result = new AuthorizationResult(AuthorizationStatus.Forbidden);

            // Act
            var isForbidden = result.IsForbidden;

            // Assert
            Assert.True(isForbidden);
        }

        [Fact]
        public void IsNoFound_ReturnsTrue_WhenResultTypeIsNoFound()
        {
            // Arrange
            var result = new AuthorizationResult(AuthorizationStatus.NoFound);

            // Act
            var isNoFound = result.IsNoFound;

            // Assert
            Assert.True(isNoFound);
        }

        [Fact]
        public void IsLocked_ReturnsTrue_WhenResultTypeIsLocked()
        {
            // Arrange
            var result = new AuthorizationResult(AuthorizationStatus.Locked);

            // Act
            var isLocked = result.IsLocked;

            // Assert
            Assert.True(isLocked);
        }

        [Fact]
        public void IsError_ReturnsTrue_WhenResultTypeIsError()
        {
            // Arrange
            var result = new AuthorizationResult(AuthorizationStatus.Error);

            // Act
            var isError = result.IsError;

            // Assert
            Assert.True(isError);
        }

        [Fact]
        public void Message_ReturnsResultTypeDescription_WhenMessageIsNull()
        {
            // Arrange
            var result = new AuthorizationResult(AuthorizationStatus.OK);

            // Act
            var message = result.Message;
            // Assert
            Assert.True(message == "权限检查通过");
        }
    }
}
