// -----------------------------------------------------------------------
//  <copyright file="MigrationPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-03 0:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;
using OSharp.Core.Packs;


namespace OSharp.Entity
{
    /// <summary>
    /// 数据迁移模块基类
    /// </summary>
    /// <typeparam name="TDbContext">数据上下文类型</typeparam>
    public abstract class MigrationPackBase<TDbContext> : OsharpPack
        where TDbContext : DbContext
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 获取 数据库类型
        /// </summary>
        protected abstract DatabaseType DatabaseType { get; }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        { 
            OsharpOptions options = provider.GetOSharpOptions();
            OsharpDbContextOptions contextOptions = options.GetDbContextOptions(typeof(TDbContext));
            if (contextOptions?.DatabaseType != DatabaseType)
            {
                return;
            }

            ILogger logger = provider.GetLogger(GetType());
            using (IServiceScope scope = provider.CreateScope())
            {
                TDbContext context = CreateDbContext(scope.ServiceProvider);
                if (context != null && contextOptions.AutoMigrationEnabled)
                {
                    context.CheckAndMigration(logger);
                    DbContextModelCache modelCache = scope.ServiceProvider.GetService<DbContextModelCache>();
                    modelCache?.Set(context.GetType(), context.Model);
                }
            }

            //初始化种子数据，只初始化当前上下文的种子数据
            IEntityManager entityManager = provider.GetService<IEntityManager>();
            Type[] entityTypes = entityManager.GetEntityRegisters(typeof(TDbContext)).Select(m => m.EntityType).Distinct().ToArray();
            IEnumerable<ISeedDataInitializer> seedDataInitializers = provider.GetServices<ISeedDataInitializer>()
                .Where(m => entityTypes.Contains(m.EntityType)).OrderBy(m => m.Order);
            foreach (ISeedDataInitializer initializer in seedDataInitializers)
            {
                initializer.Initialize();
            }
            
            IsEnabled = true;
        }

        /// <summary>
        /// 重写实现获取数据上下文实例
        /// </summary>
        /// <param name="scopedProvider">服务提供者</param>
        /// <returns></returns>
        protected abstract TDbContext CreateDbContext(IServiceProvider scopedProvider);
    }
}