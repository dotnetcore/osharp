// -----------------------------------------------------------------------
//  <copyright file="OSharpPackTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.Core.Packs
{
    /// <summary>
    /// OSharp模块类型查找器
    /// </summary>
    public class OSharpPackTypeFinder : FinderBase<Type>, ITypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="OSharpPackTypeFinder"/>类型的新实例
        /// </summary>
        public OSharpPackTypeFinder(IAllAssemblyFinder allAssemblyFinder)
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
            Type baseType = typeof(OsharpPack);
            Type[] types = AllAssemblyFinder.FindAll(true).SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                .ToArray();
            return types;
        }
    }
}