using System;

using OSharp.Reflection;

using Shouldly;

using Xunit;


namespace OSharp.Entity.Infrastructure.Tests
{
    public class EntityTypeFinderTests
    {
        [Fact]
        public void Find_Test()
        {
            EntityTypeFinder finder = new EntityTypeFinder(new AppDomainAllAssemblyFinder());
            Type[] entityTypes = finder.FindAll();
            entityTypes.ShouldContain(typeof(TestEntity));
        }

        private class TestEntity : EntityBase<int> { }
    }
}
