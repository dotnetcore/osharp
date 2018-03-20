// -----------------------------------------------------------------------
//  <copyright file="ModuleBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-07 18:49</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Reflection;


namespace OSharp.Core.Modules
{
    /// <summary>
    /// OSharp模块基类
    /// </summary>
    public abstract class OSharpModule
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public virtual ModuleLevel Level => ModuleLevel.Business;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public virtual int Order => 0;

        /// <summary>
        /// 获取 是否已可用
        /// </summary>
        public bool IsEnabled { get; protected set; }

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public virtual IServiceCollection AddServices(IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider"></param>
        public virtual void UseModule(IServiceProvider provider)
        {
            IsEnabled = true;
        }

        /// <summary>
        /// 获取当前模块的依赖模块类型
        /// </summary>
        /// <returns></returns>
        internal Type[] GetDependModuleTypes()
        {
            DependsOnModulesAttribute depends = this.GetType().GetAttribute<DependsOnModulesAttribute>();
            if (depends == null)
            {
                return new Type[0];
            }
            return depends.DependedModuleTypes;
        }
    }
}