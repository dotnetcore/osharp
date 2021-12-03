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
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
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
        /// 初始化一个<see cref="ModuleInfoPickerBase{TFunction}"/>类型的新实例
        /// </summary>
        public MvcModuleInfoPicker(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        /// <summary>
        /// 重写以实现从类型中提取模块信息
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <param name="existPaths">已存在的路径集合</param>
        /// <returns>提取到的模块信息</returns>
        protected override ModuleInfo[] GetModules(Type type, string[] existPaths)
        {
            ModuleInfoAttribute infoAttr = type.GetAttribute<ModuleInfoAttribute>();
            if (infoAttr == null)
            {
                return new ModuleInfo[0];
            }
            ModuleInfo info = new ModuleInfo()
            {
                Name = infoAttr.Name ?? GetName(type),
                Code = infoAttr.Code ?? type.Name.Replace("Controller", ""),
                Order = infoAttr.Order,
                Position = GetPosition(type, infoAttr.Position),
                PositionName = infoAttr.PositionName
            };
            List<ModuleInfo> infos = new List<ModuleInfo>() { info };
            //获取中间分类模块
            if (infoAttr.Position != null)
            {
                info = new ModuleInfo()
                {
                    Name = infoAttr.PositionName ?? infoAttr.Position,
                    Code = infoAttr.Position,
                    Position = GetPosition(type, null)
                };
                if (!existPaths.Contains($"{info.Position}.{info.Code}"))
                {
                    infos.Insert(0, info);
                }
            }
            //获取区域模块
            string area, name;
            AreaInfoAttribute areaInfo = type.GetAttribute<AreaInfoAttribute>();
            if (areaInfo != null)
            {
                area = areaInfo.RouteValue;
                name = areaInfo.Display ?? area;
            }
            else
            {
                AreaAttribute areaAttr = type.GetAttribute<AreaAttribute>();
                area = areaAttr?.RouteValue ?? "Site";
                DisplayNameAttribute display = type.GetAttribute<DisplayNameAttribute>();
                name = display?.DisplayName ?? area;
            }
            info = new ModuleInfo()
            {
                Name = name,
                Code = area,
                Position = "Root",
                PositionName = name
            };
            if (!existPaths.Contains($"{info.Position}.{info.Code}"))
            {
                infos.Insert(0, info);
            }

            return infos.ToArray();
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
            if (infoAttr == null)
            {
                return null;
            }
            ModuleInfo info = new ModuleInfo()
            {
                Name = infoAttr.Name ?? method.GetDescription() ?? method.Name,
                Code = infoAttr.Code ?? method.Name,
                Order = infoAttr.Order > 0 ? infoAttr.Order : index + 1,
            };
            string controller = method.DeclaringType?.Name.Replace("ControllerBase", string.Empty).Replace("Controller", string.Empty);
            info.Position = $"{typeInfo.Position}.{controller}";
            //依赖的功能
            string area = method.DeclaringType.GetAttribute<AreaAttribute>()?.RouteValue;
            List<IFunction> dependOnFunctions = new List<IFunction>()
            {
                FunctionHandler.GetFunction(area, controller, method.Name)
            };
            DependOnFunctionAttribute[] dependOnAttrs = method.GetAttributes<DependOnFunctionAttribute>();
            foreach (DependOnFunctionAttribute dependOnAttr in dependOnAttrs)
            {
                string dependArea = dependOnAttr.Area == null ? area : dependOnAttr.Area == string.Empty ? null : dependOnAttr.Area;
                string dependController = dependOnAttr.Controller?.Replace("Controller", string.Empty) ?? controller;
                IFunction function = FunctionHandler.GetFunction(dependArea, dependController, dependOnAttr.Action);
                if (function == null)
                {
                    throw new OsharpException($"功能“{area}/{controller}/{method.Name}”的依赖功能“{dependArea}/{dependController}/{dependOnAttr.Action}”无法找到");
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
            string area = type.GetAttribute<AreaAttribute>()?.RouteValue;
            if (area == null)
            {
                //无区域，使用Root.Site位置
                return attrPosition == null
                    ? "Root.Site"
                    : $"Root.Site.{attrPosition}";
            }
            return attrPosition == null
                ? $"Root.{area}"
                : $"Root.{area}.{attrPosition}";
        }
    }
}