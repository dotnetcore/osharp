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

using OSharp.Core.Builders;
using OSharp.Reflection;


namespace OSharp.Core.Modules
{
    /// <summary>
    /// OSharp模块管理器
    /// </summary>
    public class OSharpModuleManager
    {
        private readonly IOSharpBuilder _builder;
        private readonly OSharpModuleTypeFinder _typeFinder;
        private readonly List<OSharpModule> _sourceModules;

        /// <summary>
        /// 初始化一个<see cref="OSharpModuleManager"/>类型的新实例
        /// </summary>
        public OSharpModuleManager(IOSharpBuilder builder, IAllAssemblyFinder allAssemblyFinder)
        {
            _builder = builder;
            _typeFinder = new OSharpModuleTypeFinder(allAssemblyFinder);
            _sourceModules = new List<OSharpModule>();
            LoadedModules = new List<OSharpModule>();
        }

        /// <summary>
        /// 获取 自动检索到的所有模块信息
        /// </summary>
        public IEnumerable<OSharpModule> SourceModules
        {
            get { return _sourceModules; }
        }

        /// <summary>
        /// 获取 加载的模块信息集合
        /// </summary>
        public IEnumerable<OSharpModule> LoadedModules { get; private set; }
        
        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns></returns>
        public IServiceCollection LoadModules(IServiceCollection services)
        {
            Type[] moduleTypes = _typeFinder.FindAll();
            _sourceModules.Clear();
            _sourceModules.AddRange(moduleTypes.Select(m => Activator.CreateInstance(m) as OSharpModule));
            List<OSharpModule> modules;
            if (_builder.Modules.Any())
            {
                modules = _sourceModules.Where(m => m.IsAutoLoad)
                    .Union(_sourceModules.Where(m => _builder.Modules.Contains(m.GetType()))).Distinct().ToList();
                IEnumerable<Type> dependModuleTypes = modules.SelectMany(m => m.GetDependModuleTypes());
                modules = modules.Union(_sourceModules.Where(m => dependModuleTypes.Contains(m.GetType()))).Distinct().ToList();
                modules = modules.OrderByDescending(m => m.IsAutoLoad).ToList();
            }
            else
            {
                modules = _sourceModules.OrderByDescending(m => m.IsAutoLoad).ToList();
            }
            EnsureCoreModuleToBeFirst(modules);
            LoadedModules = modules;

            foreach (OSharpModule module in LoadedModules)
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
            foreach (OSharpModule module in LoadedModules)
            {
                module.UseModule(provider);
            }
        }

        private static void EnsureCoreModuleToBeFirst(List<OSharpModule> modules)
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