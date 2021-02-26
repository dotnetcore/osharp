// -----------------------------------------------------------------------
//  <copyright file="RouteEndpointExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-12 13:14</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization.Functions;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Authorization
{
    /// <summary>
    /// <see cref="RouteEndpoint"/>扩展方法
    /// </summary>
    public static class EndpointExtensions
    {
        /// <summary>
        /// 获取正在执行的Action相关功能信息
        /// </summary>
        public static IFunction GetExecuteFunction(this RouteEndpoint endpoint, HttpContext context)
        {
            IServiceProvider provider = context.RequestServices;
            ScopedDictionary dict = provider.GetRequiredService<ScopedDictionary>();
            if (dict.Function != null)
            {
                return dict.Function;
            }

            string area = endpoint.GetAreaName(),
                controller = endpoint.GetControllerName(),
                action = endpoint.GetActionName();
            IFunctionHandler functionHandler = provider.GetService<IFunctionHandler>();
            if (functionHandler == null)
            {
                throw new OsharpException("获取正在执行的功能时 IFunctionHandler 无法解析");
            }

            IFunction function = functionHandler.GetFunction(area, controller, action);
            if (function != null)
            {
                dict.Function = function;
            }

            return function;
        }

        /// <summary>
        /// 获取Area名
        /// </summary>
        public static string GetAreaName(this RouteEndpoint endpoint)
        {
            string area = null;
            if (endpoint.RoutePattern.RequiredValues.TryGetValue("area", out object value))
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
        public static string GetControllerName(this RouteEndpoint endpoint)
        {
            return endpoint.RoutePattern.RequiredValues["controller"].ToString();
        }

        /// <summary>
        /// 获取Action名
        /// </summary>
        public static string GetActionName(this RouteEndpoint endpoint)
        {
            return endpoint.RoutePattern.RequiredValues["action"].ToString();
        }

    }
}
