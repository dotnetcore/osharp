// -----------------------------------------------------------------------
//  <copyright file="KeyValueSeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-07 0:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Systems;
using OSharp.Dependency;
using OSharp.Entity;


namespace Liuliu.Demo.Systems
{
    [Dependency(ServiceLifetime.Singleton)]
    public class KeyValueSeedDataInitializer : SeedDataInitializerBase<KeyValue, Guid>
    {
        /// <summary>
        /// 初始化一个<see cref="SeedDataInitializerBase{TEntity, TKey}"/>类型的新实例
        /// </summary>
        public KeyValueSeedDataInitializer(IServiceProvider rootProvider)
            : base(rootProvider)
        { }

        /// <summary>
        /// 重写以提供要初始化的种子数据
        /// </summary>
        /// <returns></returns>
        protected override KeyValue[] SeedData()
        {
            return new[]
            {
                new KeyValue(SystemSettingKeys.SiteName, "OSHARP"),
                new KeyValue(SystemSettingKeys.SiteDescription, "Osharp with AspNetCore & Angular"),
            };
        }

        /// <summary>
        /// 重写以提供判断某个实体是否存在的表达式
        /// </summary>
        /// <param name="entity">要判断的实体</param>
        /// <returns></returns>
        protected override Expression<Func<KeyValue, bool>> ExistingExpression(KeyValue entity)
        {
            return m => m.Key == entity.Key;
        }
    }
}