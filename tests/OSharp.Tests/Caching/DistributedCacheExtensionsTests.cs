using System;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization.Functions;
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
            string key = "key001";
            TestEntity entity = new TestEntity(){Name = "osharp", IsDeleted = true, AddDate = DateTime.Now, Id = 1};
            _cache.Set(key, entity);
            TestEntity newEntity = _cache.Get<TestEntity>(key);
            newEntity.ShouldNotBeNull();
            newEntity.Name.ShouldBe(entity.Name);
            newEntity.AddDate.ShouldBe(entity.AddDate);
            _cache.Remove(key);
            _cache.Get<TestEntity>(key).ShouldBeNull();

            key = "key002";
            _cache.Set(key, entity, 5);
            newEntity = _cache.Get<TestEntity>(key);
            newEntity.ShouldNotBeNull();
            newEntity.Name.ShouldBe(entity.Name);
            _cache.Remove(key);
            _cache.Get<TestEntity>(key).ShouldBeNull();

            IFunction function = new Function(){CacheExpirationSeconds = 10, IsCacheSliding = false};
            key = "key003";
            _cache.Set(key, entity, function);
            newEntity = _cache.Get<TestEntity>(key);
            newEntity.ShouldNotBeNull();
            newEntity.Name.ShouldBe(entity.Name);
            _cache.Remove(key);
            _cache.Get<TestEntity>(key).ShouldBeNull();

            function.CacheExpirationSeconds = 0; //过期时间为0不缓存
            _cache.Set(key, entity, function);
            newEntity = _cache.Get<TestEntity>(key);
            newEntity.ShouldBeNull();

            key = "key004";
            newEntity = _cache.Get(key, () => entity, 10);
            newEntity.ShouldNotBeNull();
            newEntity.Name.ShouldBe(entity.Name);
            _cache.Remove(key);
            _cache.Get<TestEntity>(key).ShouldBeNull();

            function.CacheExpirationSeconds = 10;
            key = "key005";
            newEntity = _cache.Get(key, () => entity, function);
            newEntity.ShouldNotBeNull();
            newEntity.Name.ShouldBe(entity.Name);
            _cache.Remove(key);
            _cache.Get<TestEntity>(key).ShouldBeNull();
        }
    }
}
