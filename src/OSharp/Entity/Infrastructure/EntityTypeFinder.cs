// -----------------------------------------------------------------------
//  <copyright file="EntityTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-31 2:46</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using OSharp.Dependency;
using OSharp.Mapping;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体类型查找器
    /// </summary>
    public class EntityTypeFinder : IEntityTypeFinder, ISingletonDependency
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="EntityTypeFinder"/>类型的新实例
        /// </summary>
        public EntityTypeFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        public Type[] Find(Func<Type, bool> predicate, bool fromCache = false)
        {
            return FindAll(fromCache).Where(predicate).ToArray();
        }

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        public Type[] FindAll(bool fromCache = false)
        {
            Type baseType = typeof(IEntity<>);
            Assembly[] assemblies = _allAssemblyFinder.FindAll(fromCache);
            return assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsGenericAssignableFrom(type) && !type.IsAbstract)
                .Distinct().ToArray();
        }
    }
}