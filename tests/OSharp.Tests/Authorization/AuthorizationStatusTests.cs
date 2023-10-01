using OSharp.Authorization;
using OSharp.Extensions;

using Xunit;

namespace OSharp.Tests.Authorization
{
    public class AuthorizationStatusTests
    {
        [Fact]
        public void AuthorizationStatus_EnumValues_HaveCorrectDescriptions()
        {
            // Arrange
            var okValue = AuthorizationStatus.OK;
            var unauthorizedValue = AuthorizationStatus.Unauthorized;
            var forbiddenValue = AuthorizationStatus.Forbidden;
            var noFoundValue = AuthorizationStatus.NoFound;
            var lockedValue = AuthorizationStatus.Locked;
            var errorValue = AuthorizationStatus.Error;

            // Act
            var okDescription = okValue.ToDescription();
            var unauthorizedDescription = unauthorizedValue.ToDescription();
            var forbiddenDescription = forbiddenValue.ToDescription();
            var noFoundDescription = noFoundValue.ToDescription();
            var lockedDescription = lockedValue.ToDescription();
            var errorDescription = errorValue.ToDescription();

            // Assert
            Assert.Equal("权限检查通过", okDescription);
            Assert.Equal("该操作需要登录后才能继续进行", unauthorizedDescription);
            Assert.Equal("当前用户权限不足，不能继续执行", forbiddenDescription);
            Assert.Equal("指定的功能不存在", noFoundDescription);
            Assert.Equal("指定的功能被锁定", lockedDescription);
            Assert.Equal("权限检测出现错误", errorDescription);
        }
    }
}
