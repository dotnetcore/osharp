// -----------------------------------------------------------------------
//  <copyright file="SqliteMigrationPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-11-05 14:25</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;
using OSharp.Core.Packs;


namespace OSharp.Entity.Sqlite
{
    /// <summary>
    /// Sqlite数据迁移模块基类
    /// </summary>
    public abstract class SqliteMigrationPackBase<TDbContext> : OsharpPack
        where TDbContext : DbContext
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            OSharpOptions options = provider.GetOSharpOptions();
            using (IServiceScope scope = provider.CreateScope())
            {
                ILogger logger = provider.GetLogger(GetType());
                TDbContext context = CreateDbContext(scope.ServiceProvider);
                if (context != null)
                {
                    OSharpDbContextOptions contextOptions = options.GetDbContextOptions(context.GetType());
                    if (contextOptions == null)
                    {
                        logger.LogWarning($"上下文类型“{context.GetType()}”的数据库上下文配置不存在");
                        return;
                    }
                    if (contextOptions.DatabaseType != DatabaseType.Sqlite)
                    {
                        logger.LogWarning($"上下文类型“{contextOptions.DatabaseType}”不是 {nameof(DatabaseType.Sqlite)} 类型");
                        return;
                    }
                    if (contextOptions.AutoMigrationEnabled)
                    {
                        context.CheckAndMigration();
                        DbContextModelCache modelCache = scope.ServiceProvider.GetService<DbContextModelCache>();
                        if (modelCache != null)
                        {
                            modelCache.Set(context.GetType(), context.Model);
                        }
                        IsEnabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// 重写实现获取数据上下文实例
        /// </summary>
        /// <param name="scopedProvider">服务提供者</param>
        /// <returns></returns>
        protected abstract TDbContext CreateDbContext(IServiceProvider scopedProvider);
    }
}