 // -----------------------------------------------------------------------
//  <copyright file="EventBusBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-18 18:29</last-date>
// -----------------------------------------------------------------------

using System.Reflection;

using OSharp.Dependency;
using OSharp.Reflection;


namespace OSharp.EventBuses.Internal
{
    /// <summary>
    /// EventBus初始化
    /// </summary>
    internal class EventBusBuilder : IEventBusBuilder
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;
        private readonly IEventBus _eventBus;

        /// <summary>
        /// 初始化一个<see cref="EventBusBuilder"/>类型的新实例
        /// </summary>
        public EventBusBuilder(IAllAssemblyFinder allAssemblyFinder, IEventBus eventBus)
        {
            _allAssemblyFinder = allAssemblyFinder;
            _eventBus = eventBus;
        }

        /// <summary>
        /// 初始化EventBus
        /// </summary>
        public void Build()
        {
            Assembly[] assemblies = _allAssemblyFinder.FindAll(true);
            foreach (Assembly assembly in assemblies)
            {
                _eventBus.SubscribeAll(assembly);
            }
        }
    }
}