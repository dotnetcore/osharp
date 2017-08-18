// -----------------------------------------------------------------------
//  <copyright file="EntityConfigurationAssemblyFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-17 2:46</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using OSharp.Dependency;
using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体映射程序集查找器
    /// </summary>
    public class EntityConfigurationAssemblyFinder : FinderBase<Assembly>, IEntityConfigurationAssemblyFinder, ISingletonDependency
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="EntityConfigurationAssemblyFinder"/>类型的新实例
        /// </summary>
        public EntityConfigurationAssemblyFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <returns></returns>
        protected override Assembly[] FindAllItems()
        {
            Type baseType = typeof(IEntityRegister);
            Assembly[] assemblies = _allAssemblyFinder.Find(assembly => assembly.GetTypes()
                .Any(type => baseType.IsAssignableFrom(type) && !type.IsAbstract));
            return assemblies;
        }
    }
}