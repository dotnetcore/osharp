// -----------------------------------------------------------------------
//  <copyright file="OSharpModuleManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 22:32</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;
using OSharp.Reflection;


namespace OSharp.Core.Modules
{
    /// <summary>
    /// OSharp模块管理器
    /// </summary>
    public class OSharpModuleManager
    {
        private List<OSharpModule> _modules = new List<OSharpModule>();
        private readonly OSharpModuleTypeFinder _typeFinder;

        /// <summary>
        /// 初始化一个<see cref="OSharpModuleManager"/>类型的新实例
        /// </summary>
        public OSharpModuleManager(IAllAssemblyFinder allAssemblyFinder)
        {
            _typeFinder = new OSharpModuleTypeFinder(allAssemblyFinder);
        }

        /// <summary>
        /// 获取 所有模块信息
        /// </summary>
        public IReadOnlyList<OSharpModule> Modules
        {
            get { return _modules.AsReadOnly(); }
        }

        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns></returns>
        public IServiceCollection LoadModules(IServiceCollection services)
        {
            Type[] moduleTypes = _typeFinder.FindAll();
            List<OSharpModule> modules = moduleTypes.Select(m => Activator.CreateInstance(m) as OSharpModule).ToList();
            EnsureCoreModuleToBeFirst(modules);
            _modules = modules;

            foreach (OSharpModule module in modules)
            {
                services = module.AddServices(services);
            }

            return services;
        }

        /// <summary>
        /// 启用模块
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public void UseModules(IServiceProvider provider)
        {
            foreach (OSharpModule module in Modules)
            {
                module.UseServices(provider);
            }
        }

        private void EnsureCoreModuleToBeFirst(List<OSharpModule> modules)
        {
            int index = modules.FindIndex(m => m.GetType() == typeof(OSharpCoreModule));
            if (index <= 0)
            {
                return;
            }
            OSharpModule coreModule = modules[index];
            modules.RemoveAt(index);
            modules.Insert(0, coreModule);
        }
    }
}