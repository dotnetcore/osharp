// -----------------------------------------------------------------------
//  <copyright file="ModuleInfoPickerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:13</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Authorization.Functions;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Reflection;


namespace OSharp.Authorization.Modules
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
        protected ModuleInfoPickerBase(IServiceProvider serviceProvider)
        {
            Logger = serviceProvider.GetLogger(GetType());
            FunctionHandler = serviceProvider.GetService<IFunctionHandler>();
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
            Logger.LogInformation("开始提取Module模块信息");
            Check.NotNull(FunctionHandler, nameof(FunctionHandler));
            Type[] moduleTypes = FunctionHandler.GetAllFunctionTypes().Where(type => type.HasAttribute<ModuleInfoAttribute>()).ToArray();
            ModuleInfo[] modules = GetModules(moduleTypes);
            Logger.LogInformation($"提取到 {modules.Length} 个Module模块信息");
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
                string[] existPaths = infos.Select(m => $"{m.Position}.{m.Code}").ToArray();
                ModuleInfo[] typeInfos = GetModules(moduleType, existPaths);
                foreach (ModuleInfo info in typeInfos)
                {
                    if (info.Order.Equals(0))
                    {
                        info.Order = infos.Count(m => m.Position == info.Position) + 1;
                    }
                    if (!infos.Contains(info))
                    {
                        infos.Add(info);
                        Logger.LogDebug($"提取模块信息：{info.Name}[{info.Code}]({info.Position}),FunctionCount:{info.DependOnFunctions.Length}");
                    }
                }

                MethodInfo[] methods = FunctionHandler.GetMethodInfos(moduleType).Where(type => type.HasAttribute<ModuleInfoAttribute>()).ToArray();
                for (int index = 0; index < methods.Length; index++)
                {
                    ModuleInfo info = GetModule(methods[index], typeInfos.Last(), index);
                    if (info != null)
                    {
                        infos.Add(info);
                        Logger.LogDebug($"提取模块信息：{info.Name}[{info.Code}]({info.Position}),FunctionCount:{info.DependOnFunctions.Length}");
                    }
                }
            }

            return infos.ToArray();
        }

        /// <summary>
        /// 重写以实现从类型中提取模块信息
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <param name="existPaths">已存在的路径集合</param>
        /// <returns>提取到的模块信息</returns>
        protected abstract ModuleInfo[] GetModules(Type type, string[] existPaths);

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