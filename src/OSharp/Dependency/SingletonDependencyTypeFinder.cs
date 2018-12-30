// -----------------------------------------------------------------------
//  <copyright file="SingletonDependencyTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 22:00</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.Dependency
{
    /// <summary>
    /// <see cref="ServiceLifetime.Singleton"/>生命周期类型的服务映射类型查找器
    /// </summary>
    public class SingletonDependencyTypeFinder : FinderBase<Type>, ISingletonDependencyTypeFinder
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="SingletonDependencyTypeFinder"/>类型的新实例
        /// </summary>
        public SingletonDependencyTypeFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <inheritdoc />
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(ISingletonDependency);
            Type[] types = _allAssemblyFinder.FindAll(fromCache: true).SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && baseType.IsAssignableFrom(type) && !type.HasAttribute<IgnoreDependencyAttribute>() && !type.IsAbstract)
                .ToArray();
            return types;
        }
    }
}