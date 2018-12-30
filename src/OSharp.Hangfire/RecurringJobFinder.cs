// -----------------------------------------------------------------------
//  <copyright file="RecurringJobFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 17:56</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.Hangfire
{
    public class RecurringJobFinder : FinderBase<Type>, IRecurringJobFinder
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="RecurringJobFinder"/>类型的新实例
        /// </summary>
        public RecurringJobFinder(IAllAssemblyFinder allAssemblyFinder)
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
                .Where(type => type.IsDeriveClassFrom<IRecurringJob>()).Distinct().ToArray();
        }
    }
}