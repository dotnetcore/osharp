using System;

using Shouldly;

using Xunit;


namespace OSharp.Entity.Tests
{
    public class ExpirableBaseTests
    {
        [Fact]
        public void Ctor_Test()
        {
            TestEntity entity = new TestEntity();
            entity.BeginTime.ShouldBeNull();
            entity.EndTime.ShouldBeNull();
        }

        [Fact]
        public void ThrowIfTimeInvalid_Test()
        {
            TestEntity entity = new TestEntity
            {
                BeginTime = DateTime.Now.AddDays(-1),
                EndTime = DateTime.Now.AddDays(1)
            };
            entity.ThrowIfTimeInvalid();

            entity.BeginTime = DateTime.Now.AddDays(2);
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                entity.ThrowIfTimeInvalid();
            });
        }

        private class TestEntity : ExpirableBase<int> { }
    }
}
