// -----------------------------------------------------------------------
//  <copyright file="EntityFrameworkCorePack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-14 15:57</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Authorization.EntityInfos;
using OSharp.Collections;
using OSharp.Core.Options;
using OSharp.Core.Packs;
using OSharp.Data.Snows;
using OSharp.Entity.Internal;
using OSharp.Entity.KeyGenerate;
using OSharp.EventBuses;
using OSharp.Exceptions;


namespace OSharp.Entity
{
    /// <summary>
    /// EntityFrameworkCore基模块
    /// </summary>
    [DependsOnPacks(typeof(EventBusPack))]
    public abstract class EntityFrameworkCorePackBase : OsharpPack
    {
        private static bool _optionsValidated;

        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 获取 数据库类型
        /// </summary>
        protected abstract DatabaseType DatabaseType { get; }

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddScoped<IAuditEntityProvider, AuditEntityProvider>();
            services.TryAddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.TryAddScoped<IConnectionStringProvider, ConnectionStringProvider>();
            services.TryAddScoped<IMasterSlaveSplitPolicy, MasterSlaveSplitPolicy>();

            services.TryAddSingleton<IKeyGenerator<int>, AutoIncreaseKeyGenerator>();
            services.TryAddSingleton<IKeyGenerator<long>>(new SnowKeyGenerator(new DefaultIdGenerator(new IdGeneratorOptions(1))));
            services.TryAddSingleton<IEntityManager, EntityManager>();
            services.AddSingleton<DbContextModelCache>();
            services.AddSingleton<IEntityBatchConfiguration, TableNamePrefixConfiguration>();
            services.AddSingleton<ISlaveDatabaseSelector, RandomSlaveDatabaseSelector>();
            services.AddSingleton<ISlaveDatabaseSelector, SequenceSlaveDatabaseSelector>();
            services.AddSingleton<ISlaveDatabaseSelector, WeightSlaveDatabaseSelector>();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            IDictionary<string, OsharpDbContextOptions> dbContextOptions = provider.GetOSharpOptions().DbContexts;
            if (!_optionsValidated)
            {
                if (dbContextOptions.IsNullOrEmpty())
                {
                    throw new OsharpException("配置文件中找不到数据上下文的配置，请配置OSharp:DbContexts节点");
                }

                foreach (var options in dbContextOptions)
                {
                    string msg = options.Value.Error;
                    if (msg != string.Empty)
                    {
                        throw new OsharpException($"数据库“{options.Key}”配置错误：{msg}");
                    }
                }

                _optionsValidated = true;
            }

            if (dbContextOptions.Values.All(m => m.DatabaseType != DatabaseType))
            {
                return;
            }

            IEntityManager manager = provider.GetRequiredService<IEntityManager>();
            manager.Initialize();
            IsEnabled = true;
        }
    }
}