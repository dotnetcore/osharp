// -----------------------------------------------------------------------
//  <copyright file="OSharpModuleTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 22:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using OSharp.Reflection;
using OSharp.Finders;


namespace OSharp.Core.Modules
{
    /// <summary>
    /// OSharp模块类型查找器
    /// </summary>
    public class OSharpModuleTypeFinder : FinderBase<Type>, ITypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="OSharpModuleTypeFinder"/>类型的新实例
        /// </summary>
        public OSharpModuleTypeFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            AllAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 获取或设置 全部程序集查找器
        /// </summary>
        public IAllAssemblyFinder AllAssemblyFinder { get; set; }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(OSharpModule);
            Type[] types = AllAssemblyFinder.FindAll(true).SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                .ToArray();
            return types;
        }
    }
}