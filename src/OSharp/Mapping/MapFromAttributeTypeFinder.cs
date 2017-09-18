// -----------------------------------------------------------------------
//  <copyright file="MapFromAttributeTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-14 0:06</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using OSharp.Dependency;
using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.Mapping
{
    /// <inheritdoc cref="IMapFromAttributeTypeFinder" />
    public class MapFromAttributeTypeFinder : FinderBase<Type>, IMapFromAttributeTypeFinder, ISingletonDependency
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="MapFromAttributeTypeFinder"/>类型的新实例
        /// </summary>
        public MapFromAttributeTypeFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Assembly[] assemblies = _allAssemblyFinder.FindAll(true);
            return assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && type.HasAttribute<MapFromAttribute>() && !type.IsAbstract)
                .Distinct().ToArray();
        }
    }
}