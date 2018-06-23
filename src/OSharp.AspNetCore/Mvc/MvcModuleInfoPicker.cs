// -----------------------------------------------------------------------
//  <copyright file="MvcModuleInfoPicker.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 17:23</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;

using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Exceptions;
using OSharp.Reflection;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// MVC模块信息提取器
    /// </summary>
    public class MvcModuleInfoPicker : ModuleInfoPickerBase<Function>
    {
        /// <summary>
        /// 重写以实现从类型中提取模块信息
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <returns>提取到的模块信息</returns>
        protected override ModuleInfo GetModule(Type type)
        {
            ModuleInfoAttribute infoAttr = type.GetAttribute<ModuleInfoAttribute>();
            ModuleInfo info = new ModuleInfo()
            {
                Name = infoAttr.Name ?? GetName(type),
                Code = infoAttr.Code ?? type.Name.Replace("Controller", ""),
                Order = infoAttr.Order,
                Position = GetPosition(type, infoAttr.Position)
            };
            return info;
        }

        /// <summary>
        /// 重写以实现从方法信息中提取模块信息
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="typeInfo">所在类型模块信息</param>
        /// <param name="index">序号</param>
        /// <returns>提取到的模块信息</returns>
        protected override ModuleInfo GetModule(MethodInfo method, ModuleInfo typeInfo, int index)
        {
            ModuleInfoAttribute infoAttr = method.GetAttribute<ModuleInfoAttribute>();
            ModuleInfo info = new ModuleInfo()
            {
                Name = infoAttr.Name ?? method.GetDescription() ?? method.Name,
                Code = infoAttr.Code ?? method.Name,
                Order = infoAttr.Order > 0 ? infoAttr.Order : index + 1
            };
            string controller = method.DeclaringType?.Name.Replace("Controller", "");
            info.Position = $"{typeInfo.Position}.{controller}";
            //依赖的功能
            string area = method.DeclaringType.GetAttribute<AreaAttribute>(true)?.RouteValue;
            List<IFunction> dependOnFunctions = new List<IFunction>()
            {
                FunctionHandler.GetFunction(area, controller, method.Name)
            };
            DependOnFunctionAttribute[] dependOnAttrs = method.GetAttributes<DependOnFunctionAttribute>();
            foreach (DependOnFunctionAttribute dependOnAttr in dependOnAttrs)
            {
                string darea = dependOnAttr.Area == null ? area : dependOnAttr.Area == string.Empty ? null : dependOnAttr.Area;
                string dcontroller = dependOnAttr.Controller ?? controller;
                IFunction function = FunctionHandler.GetFunction(darea, dcontroller, dependOnAttr.Action);
                if (function == null)
                {
                    throw new OsharpException($"功能“{area}/{controller}/{method.Name}”的依赖功能“{darea}/{dcontroller}/{dependOnAttr.Action}”无法找到");
                }
                dependOnFunctions.Add(function);
            }
            info.DependOnFunctions = dependOnFunctions.ToArray();

            return info;
        }

        private static string GetName(Type type)
        {
            string name = type.GetDescription();
            if (name == null)
            {
                return type.Name.Replace("Controller", "");
            }
            if (name.Contains("-"))
            {
                name = name.Split('-').Last();
            }
            return name;
        }

        private static string GetPosition(Type type, string attrPosition)
        {
            string area = type.GetAttribute<AreaAttribute>(true)?.RouteValue;
            if (area == null)
            {
                //无区域，使用Root.Site位置
                return attrPosition == null
                    ? $"Root.Site"
                    : $"Root.Site.{attrPosition}";
            }
            return attrPosition == null
                ? $"Root.{area}"
                : $"Root.{area}.{attrPosition}";
        }
    }
}