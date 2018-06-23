// -----------------------------------------------------------------------
//  <copyright file="ModuleHandlerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 16:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Extensions.Logging;

using OSharp.Core.Functions;
using OSharp.Entity;
using OSharp.Reflection;


namespace OSharp.Core.Modules
{
    /// <summary>
    /// 模块信息提取器基类
    /// </summary>
    public abstract class ModuleInfoPickerBase<TFunction> : IModuleInfoPicker
        where TFunction : class, IEntity<Guid>, IFunction, new()
    {
        /// <summary>
        /// 初始化一个<see cref="ModuleInfoPickerBase{TFunction}"/>类型的新实例
        /// </summary>
        protected ModuleInfoPickerBase()
        {
            ServiceLocator locator = ServiceLocator.Instance;
            Logger = locator.GetService<ILoggerFactory>().CreateLogger(GetType());
            FunctionHandler = locator.GetService<IFunctionHandler>();
        }

        /// <summary>
        /// 获取 日志记录对象
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 获取 功能处理器
        /// </summary>
        protected IFunctionHandler FunctionHandler { get; }

        /// <summary>
        /// 从程序集中获取模块信息
        /// </summary>
        public ModuleInfo[] Pickup()
        {
            Check.NotNull(FunctionHandler, nameof(FunctionHandler));
            Type[] moduleTypes = FunctionHandler.FunctionTypeFinder.Find(type => type.HasAttribute<ModuleInfoAttribute>());
            ModuleInfo[] modules = GetModules(moduleTypes);
            return modules;
        }

        /// <summary>
        /// 重写以实现从类型中提取模块信息
        /// </summary>
        /// <param name="moduleTypes"></param>
        /// <returns></returns>
        protected virtual ModuleInfo[] GetModules(Type[] moduleTypes)
        {
            List<ModuleInfo> infos = new List<ModuleInfo>();
            foreach (Type moduleType in moduleTypes)
            {
                ModuleInfo typeInfo = GetModule(moduleType);
                infos.Add(typeInfo);
                MethodInfo[] methods = FunctionHandler.MethodInfoFinder.Find(moduleType, type => type.HasAttribute<ModuleInfoAttribute>());
                for (int index = 0; index < methods.Length; index++)
                {
                    ModuleInfo methodInfo = GetModule(methods[index], typeInfo, index);
                    infos.Add(methodInfo);
                }
            }
            return infos.ToArray();
        }

        /// <summary>
        /// 重写以实现从类型中提取模块信息
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <returns>提取到的模块信息</returns>
        protected abstract ModuleInfo GetModule(Type type);

        /// <summary>
        /// 重写以实现从方法信息中提取模块信息
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="typeInfo">所在类型模块信息</param>
        /// <param name="index">序号</param>
        /// <returns>提取到的模块信息</returns>
        protected abstract ModuleInfo GetModule(MethodInfo method, ModuleInfo typeInfo, int index);
    }
}