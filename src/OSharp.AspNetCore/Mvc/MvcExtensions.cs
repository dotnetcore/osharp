// -----------------------------------------------------------------------
//  <copyright file="MvcExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-15 1:41</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;


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
    }
}