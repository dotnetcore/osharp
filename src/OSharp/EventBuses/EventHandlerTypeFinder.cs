// -----------------------------------------------------------------------
//  <copyright file="EventHandlerTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-28 23:23</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 事件处理器类型查找器
    /// </summary>
    public class EventHandlerTypeFinder : FinderBase<Type>, IEventHandlerTypeFinder
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="EventHandlerTypeFinder"/>类型的新实例
        /// </summary>
        public EventHandlerTypeFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(IEventHandler<>);
            return _allAssemblyFinder.FindAll(true).SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsDeriveClassFrom(baseType)).Distinct().ToArray();
        }
    }
}