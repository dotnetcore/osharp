using System;

using Shouldly;

using Xunit;

namespace OSharp.Entity.Tests
{
    public class EntityInterfaceExtensionsTests
    {
        [Fact]
        public void IsEntityType_Test()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                ((Type)null).IsEntityType();
            });
            GetType().IsEntityType().ShouldBeFalse();
            typeof(TestEntity).IsEntityType().ShouldBeTrue();
        }

        [Fact]
        public void IsExpired_Test()
        {
            TestEntity entity = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                entity.IsExpired();
            });

            entity = new TestEntity() { CreatedTime = DateTime.Now.AddDays(-1), EndTime = DateTime.Now.AddDays(1) };
            entity.IsExpired().ShouldBeFalse();

            entity.EndTime = DateTime.Now.AddDays(-0.5);
            entity.IsExpired().ShouldBeTrue();
        }

        private class TestEntity : ExpirableBase<int>, ICreatedTime
        {
            /// <summary>
            /// 获取或设置 创建时间
            /// </summary>
            public DateTime CreatedTime { get; set; }
        }
    }
}
