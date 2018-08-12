// -----------------------------------------------------------------------
//  <copyright file="DbContextResolver.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-21 1:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Entity.Transactions;
using OSharp.Exceptions;


namespace OSharp.Entity
{
    /// <summary>
    /// 数据上下文对象解析器
    /// </summary>
    public class DbContextResolver : IDbContextResolver
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="DbContextResolver"/>类型的新实例
        /// </summary>
        public DbContextResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取指定类型的数据上下文对象
        /// </summary>
        /// <param name="resolveOptions">上下文解析选项</param>
        /// <returns></returns>
        public IDbContext Resolve(DbContextResolveOptions resolveOptions)
        {
            Type dbContextType = resolveOptions.DbContextType;
            IDbContextOptionsBuilderCreator builderCreator = _serviceProvider.GetServices<IDbContextOptionsBuilderCreator>()
                .FirstOrDefault(m => m.Type == resolveOptions.DatabaseType);
            if (builderCreator == null)
            {
                throw new OsharpException($"无法解析类型为“{resolveOptions.DatabaseType}”的 {typeof(IDbContextOptionsBuilderCreator).FullName} 实例");
            }
            DbContextOptionsBuilder optionsBuilder = builderCreator.Create(resolveOptions.ConnectionString, resolveOptions.ExistingConnection);
            DbContextModelCache modelCache = _serviceProvider.GetService<DbContextModelCache>();
            IModel model = modelCache.Get(dbContextType);
            if (model != null)
            {
                optionsBuilder.UseModel(model);
            }
            DbContextOptions options = optionsBuilder.Options;

            //创建上下文实例
            if (!(ActivatorUtilities.CreateInstance(_serviceProvider, dbContextType, options) is DbContext context))
            {
                throw new OsharpException($"实例化数据上下文“{dbContextType.AssemblyQualifiedName}”失败");
            }
            return context as IDbContext;
        }
    }
}