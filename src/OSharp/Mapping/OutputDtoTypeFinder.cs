// -----------------------------------------------------------------------
//  <copyright file="OutputDtoTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-26 13:55</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.Mapping
{
    /// <summary>
    /// 输出DTO类型查找器
    /// </summary>
    public class OutputDtoTypeFinder : FinderBase<Type>, IMapTargetTypeFinder, ISingletonDependency
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="OutputDtoTypeFinder"/>类型的新实例
        /// </summary>
        public OutputDtoTypeFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Assembly[] assemblies = _allAssemblyFinder.FindAll();
            return assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IOutputDto).IsAssignableFrom(type) && !type.IsAbstract)
                .Distinct().ToArray();
        }
    }
}