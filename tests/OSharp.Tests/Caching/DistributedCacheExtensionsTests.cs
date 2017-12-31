using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OSharp.UnitTest.Infrastructure;

using Shouldly;

using Xunit;


namespace OSharp.Caching.Tests
{
    public class DistributedCacheExtensionsTests
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheExtensionsTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            IServiceProvider provider = services.BuildServiceProvider();
            IDistributedCache cache = provider.GetService<IDistributedCache>();
            cache.ShouldNotBeNull();
            _cache = cache;
        }

        [Fact]
        public void Set_Get_Test()
        {
            string key = "test-key";
            TestEntity entity = new TestEntity(){Name = "osharp", IsDeleted = true, AddDate = DateTime.Now, Id = 1};
            _cache.Set(key, entity);
            TestEntity newEntity = _cache.Get<TestEntity>(key);
            newEntity.ShouldNotBeNull();
            newEntity.Name.ShouldBe(entity.Name);
            newEntity.AddDate.ShouldBe(entity.AddDate);
            _cache.Remove(key);
            _cache.Get<TestEntity>(key).ShouldBeNull();
        }
    }
}
