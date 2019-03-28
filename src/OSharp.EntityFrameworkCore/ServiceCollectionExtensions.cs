// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-27 13:23</last-date>
// -----------------------------------------------------------------------

using System;
using System.Data.Common;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 依赖注入服务集合扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 将基于Osharp数据上下文基类<see cref="DbContextBase"/>上下文类型添加到服务集合中
        /// </summary>
        /// <typeparam name="TDbContext">基于Osharp数据上下文基类<see cref="DbContextBase"/>上下文类型</typeparam>
        /// <param name="services">依赖注入服务集合</param>
        /// <param name="optionsAction">数据库选项创建配置，将在内置配置后运行</param>
        /// <returns>依赖注入服务集合</returns>
        public static IServiceCollection AddOsharpDbContext<TDbContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = null) where TDbContext : DbContextBase
        {
            services.AddDbContext<TDbContext>((provider, builder) =>
            {
                Type dbContextType = typeof(TDbContext);
                OsharpOptions osharpOptions = provider.GetOSharpOptions();
                OsharpDbContextOptions osharpDbContextOptions = osharpOptions?.GetDbContextOptions(dbContextType);
                if (osharpDbContextOptions == null)
                {
                    throw new OsharpException($"无法找到数据上下文“{dbContextType.DisplayName()}”的配置信息");
                }

                //启用延迟加载
                if (osharpDbContextOptions.LazyLoadingProxiesEnabled)
                {
                    builder = builder.UseLazyLoadingProxies();
                }
                DatabaseType databaseType = osharpDbContextOptions.DatabaseType;

                //处理数据库驱动差异处理
                IDbContextOptionsBuilderDriveHandler driveHandler = provider.GetServices<IDbContextOptionsBuilderDriveHandler>()
                    .FirstOrDefault(m => m.Type == databaseType);
                if (driveHandler == null)
                {
                    throw new OsharpException($"无法解析类型为“{databaseType}”的 {typeof(IDbContextOptionsBuilderDriveHandler).DisplayName()} 实例");
                }

                ScopedDictionary scopedDictionary = provider.GetService<ScopedDictionary>();
                string key = $"DnConnection_{osharpDbContextOptions.ConnectionString}";
                DbConnection existingDbConnection = scopedDictionary.GetValue<DbConnection>(key);
                builder = driveHandler.Handle(builder, osharpDbContextOptions.ConnectionString, existingDbConnection);

                //使用模型缓存
                DbContextModelCache modelCache = provider.GetService<DbContextModelCache>();
                IModel model = modelCache?.Get(dbContextType);
                if (model != null)
                {
                    builder = builder.UseModel(model);
                }

                //额外的选项
                optionsAction?.Invoke(provider, builder);
            });
            return services;
        }
    }
}