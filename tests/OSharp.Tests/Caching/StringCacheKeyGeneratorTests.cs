using System;

using Shouldly;

using Xunit;


namespace OSharp.Caching.Tests
{
    public class StringCacheKeyGeneratorTests
    {
        [Fact]
        public void GetKeyTest()
        {
            ICacheKeyGenerator generator = new StringCacheKeyGenerator();
            Assert.Throws<ArgumentException>(() =>
            {
                generator.GetKey();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                generator.GetKey(new object[0]);
            });

            generator.GetKey("abc", 123, false).ShouldBe("abc-123-False");
        }
    }
}
