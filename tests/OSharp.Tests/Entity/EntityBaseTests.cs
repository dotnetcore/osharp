using System;

using Shouldly;

using Xunit;


namespace OSharp.Entity.Tests
{
    public class EntityBaseTests
    {
        [Fact]
        public void Ctor_Test()
        {
            GuidKeyEntity guidKeyEntity = new GuidKeyEntity();
            guidKeyEntity.Id.ShouldBeOfType<Guid>();
            guidKeyEntity.Id.ShouldBe(Guid.Empty);
        }

        [Fact]
        public void Equals_Test()
        {
            TestEntity1 entity1 = new TestEntity1() { Id = 1 };
            TestEntity1 entity2 = new TestEntity1() { Id = 1 };
            entity2.Equals(entity1).ShouldBeTrue();
            entity2.Equals(this).ShouldBeFalse();
            entity2.GetHashCode().ShouldBe(entity1.GetHashCode());
        }

        private class GuidKeyEntity : EntityBase<Guid> { }


        private class TestEntity1 : EntityBase<int> { }
    }
}
