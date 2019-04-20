// -----------------------------------------------------------------------
//  <copyright file="InputDtoTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-06 10:18</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// <see cref="IInputDto{T}"/>类型查找器
    /// </summary>
    public class InputDtoTypeFinder : FinderBase<Type>, IInputDtoTypeFinder
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="InputDtoTypeFinder"/>类型的新实例
        /// </summary>
        public InputDtoTypeFinder(IAllAssemblyFinder allAssemblyFinder)
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
                .Where(type => type.IsDeriveClassFrom(typeof(IInputDto<>))).Distinct().ToArray();
        }
    }
}