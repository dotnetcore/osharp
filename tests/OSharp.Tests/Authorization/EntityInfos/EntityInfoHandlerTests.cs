using Xunit;
using OSharp.Authorization.EntityInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NSubstitute;

using OSharp.Entity;

using Shouldly;


namespace OSharp.Authorization.EntityInfos.Tests
{
    public class EntityInfoHandlerTests
    {
        private readonly EntityInfoHandler _handler;
        private ILogger _logger;

        public EntityInfoHandlerTests()
        {
            IServiceProvider serviceProvider = GetServiceProvider();
            _handler = new EntityInfoHandler(serviceProvider);
        }

        private IServiceProvider GetServiceProvider()
        {
            _logger = Substitute.For<ILogger<EntityInfoHandler>>();
            var factory = Substitute.For<ILoggerFactory>();

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService<ILoggerFactory>().Returns(factory);
            factory.CreateLogger<EntityInfoHandler>().Returns(_logger);
            return serviceProvider;
        }

        [Fact]
        public void SyncToDatabaseTest_仓储不存在()
        {
            IServiceProvider rootProvider = GetServiceProvider();
            EntityInfoHandlerProtected handler = new EntityInfoHandlerProtected(rootProvider);

            IServiceProvider scopedProvider = Substitute.For<IServiceProvider>();
            List<EntityInfo>entityInfos = new List<EntityInfo>();
            handler.ExposedSyncToDatabase(scopedProvider, entityInfos);
        }

        [Fact]
        public void SyncToDatabaseTest_签名相同()
        {
            IServiceProvider rootProvider = GetServiceProvider();
            EntityInfoHandlerProtected handler = new EntityInfoHandlerProtected(rootProvider);

            IServiceProvider scopedProvider = Substitute.For<IServiceProvider>();
            IRepository<EntityInfo, Guid> repository = Substitute.For<IRepository<EntityInfo, Guid>>();
            scopedProvider.GetService<IRepository<EntityInfo, Guid>>().Returns(repository);

            List<EntityInfo> entityInfos = Substitute.For<List<EntityInfo>>();
            //entityInfos.CheckSyncByHash(scopedProvider, _logger).Returns(false);

            handler.ExposedSyncToDatabase(scopedProvider, entityInfos);
        }
        
        [Fact]
        public void GetFromDatabaseTest()
        {
            IServiceProvider rootProvider = GetServiceProvider();
            EntityInfoHandlerProtected handler = new EntityInfoHandlerProtected(rootProvider);

            IServiceProvider scopedProvider = Substitute.For<IServiceProvider>();
            EntityInfo[] entityInfos = handler.ExposedGetFromDatabase(scopedProvider);
            entityInfos.ShouldBeEmpty();

            IRepository<EntityInfo, Guid> repository = Substitute.For<IRepository<EntityInfo, Guid>>();
            repository.QueryAsNoTracking(null, false).Returns(new [] { new EntityInfo() }.AsQueryable());
            scopedProvider.GetService<IRepository<EntityInfo, Guid>>().Returns(repository);
            entityInfos = handler.ExposedGetFromDatabase(scopedProvider);
            entityInfos.ShouldNotBeEmpty();
            entityInfos.Length.ShouldBe(1);
        }

        private class EntityInfoHandlerProtected : EntityInfoHandlerBase<EntityInfo, EntityInfoHandlerProtected>
        {
            /// <summary>
            /// 初始化一个<see cref="EntityInfoHandlerBase{TEntityInfo,TEntityInfoProvider}"/>类型的新实例
            /// </summary>
            public EntityInfoHandlerProtected(IServiceProvider serviceProvider)
                : base(serviceProvider)
            { }

            public void ExposedSyncToDatabase(IServiceProvider provider, List<EntityInfo> entityInfos)
            {
                base.SyncToDatabase(provider, entityInfos);
            }

            public EntityInfo[] ExposedGetFromDatabase(IServiceProvider scopedProvider)
            {
                return base.GetFromDatabase(scopedProvider);
            }
        }
    }
}