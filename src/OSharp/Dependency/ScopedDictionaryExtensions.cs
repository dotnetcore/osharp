// -----------------------------------------------------------------------
//  <copyright file="ScopedDictionaryExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-27 14:54</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Entity;


namespace OSharp.Dependency
{
    /// <summary>
    /// Scoped生命周期字典扩展方法
    /// </summary>
    public static class ScopedDictionaryExtensions
    {
        /// <summary>
        /// 获取连接串的UnitOfWork
        /// </summary>
        public static IUnitOfWork GetConnUnitOfWork(this ScopedDictionary dict, string connString)
        {
            string key = $"UnitOfWork_ConnString_{connString}";
            return dict.GetValue<IUnitOfWork>(key);
        }

        /// <summary>
        /// 获取所有连接串的UnitOfWork
        /// </summary>
        public static IUnitOfWork[] GetConnUnitOfWorks(this ScopedDictionary dict)
        {
            return dict.Where(m => m.Key.StartsWith("UnitOfWork_ConnString_")).Select(m => m.Value as IUnitOfWork).ToArray();
        }

        /// <summary>
        /// 设置连接串的UnitOfWork
        /// </summary>
        public static void SetConnUnitOfWork(this ScopedDictionary dict, string connString, IUnitOfWork unitOfWork)
        {
            string key = $"UnitOfWork_ConnString_{connString}";
            dict.TryAdd(key, unitOfWork);
        }

        /// <summary>
        /// 获取指定实体类的UnitOfWork
        /// </summary>
        public static IUnitOfWork GetEntityUnitOfWork(this ScopedDictionary dict, Type entityType)
        {
            string key = $"UnitOfWork_EntityType_{entityType.FullName}";
            return dict.GetValue<IUnitOfWork>(key);
        }

        /// <summary>
        /// 获取所有实体类的UnitOfWork
        /// </summary>
        public static IUnitOfWork[] GetEntityUnitOfWorks(this ScopedDictionary dict)
        {
            return dict.Where(m => m.Key.StartsWith("UnitOfWork_EntityType_")).Select(m => m.Value as IUnitOfWork).ToArray();
        }

        /// <summary>
        /// 设置指定实体类的UnitOfWork
        /// </summary>
        public static void SetEntityUnitOfWork(this ScopedDictionary dict, Type entityType, IUnitOfWork unitOfWork)
        {
            string key = $"UnitOfWork_EntityType_{entityType.FullName}";
            dict.TryAdd(key, unitOfWork);
        }
    }
}