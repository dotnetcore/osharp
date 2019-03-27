// -----------------------------------------------------------------------
//  <copyright file="DbContextModelCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-12 14:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;
using OSharp.Extensions;


namespace OSharp.Entity
{
    /// <summary>
    /// 上下文数据模型缓存
    /// </summary>
    [Dependency(ServiceLifetime.Singleton, AddSelf = true)]
    public class DbContextModelCache
    {
        private readonly ConcurrentDictionary<Type, IModel> _dict = new ConcurrentDictionary<Type, IModel>();

        /// <summary>
        /// 获取指定上下文类型的模型
        /// </summary>
        /// <param name="dbContextType">上下文类型</param>
        /// <returns>数据模型</returns>
        public IModel Get(Type dbContextType)
        {
            return _dict.GetOrDefault(dbContextType);
        }

        /// <summary>
        /// 设置指定上下文类型的模型
        /// </summary>
        /// <param name="dbContextType">上下文类型</param>
        /// <param name="model">模型</param>
        public void Set(Type dbContextType, IModel model)
        {
            _dict[dbContextType] = model;
        }

        /// <summary>
        /// 移除指定上下文类型的模型
        /// </summary>
        public void Remove(Type dbContextType)
        {
            _dict.TryRemove(dbContextType, out IModel model);
        }
    }
}