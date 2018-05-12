// -----------------------------------------------------------------------
//  <copyright file="MvcExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-15 1:41</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;

using OSharp.Core.Functions;
using OSharp.Exceptions;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// MVC相关扩展方法
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// 判断类型是否是Controller
        /// </summary>
        public static bool IsController(this Type type)
        {
            return IsController(type.GetTypeInfo());
        }

        /// <summary>
        /// 判断类型是否是Controller
        /// </summary>
        public static bool IsController(this TypeInfo typeInfo)
        {
            return typeInfo.IsClass && !typeInfo.IsAbstract && (typeInfo.IsPublic && !typeInfo.ContainsGenericParameters)
                && (!typeInfo.IsDefined(typeof(NonControllerAttribute)) && (typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                    || typeInfo.IsDefined(typeof(ControllerAttribute))));
        }

        /// <summary>
        /// 获取Area名
        /// </summary>
        public static string GetAreaName(this ActionContext context)
        {
            string area = null;
            if (context.RouteData.Values.TryGetValue("area", out object value))
            {
                area = (string)value;
                if (area.IsNullOrWhiteSpace())
                {
                    area = null;
                }
            }
            return area;
        }

        /// <summary>
        /// 获取Controller名
        /// </summary>
        public static string GetControllerName(this ActionContext context)
        {
            return context.RouteData.Values["controller"].ToString();
        }

        /// <summary>
        /// 获取Action名
        /// </summary>
        public static string GetActionName(this ActionContext context)
        {
            return context.RouteData.Values["action"].ToString();
        }

        /// <summary>
        /// 获取正在执行的Action的相关功能信息
        /// </summary>
        public static IFunction GetExecuteFunction(this ActionContext context)
        {
            const string key = OsharpConstants.CurrentMvcFunctionKey;
            IDictionary<object, object> items = context.HttpContext.Items;
            if (items.ContainsKey(key))
            {
                return items[key] as IFunction;
            }
            string area = context.GetAreaName();
            string controller = context.GetControllerName();
            string action = context.GetActionName();
            IMvcFunctionHandler functionHandler = ServiceLocator.Instance.GetService<IMvcFunctionHandler>();
            if (functionHandler == null)
            {
                throw new OsharpException("获取正在执行的功能时 IMvcFunctionHandler 无法解析");
            }
            IFunction function = functionHandler.GetFunction(area, controller, action);
            if (function != null)
            {
                items.Add(key, function);
            }
            return function;
        }

        /// <summary>
        /// 获取当前Controller中正在操作的Action的相关功能信息
        /// </summary>
        public static IFunction GetExecuteFunction(this ControllerBase controller)
        {
            return controller.ControllerContext.GetExecuteFunction();
        }
    }
}