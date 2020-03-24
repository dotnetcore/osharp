// -----------------------------------------------------------------------
//  <copyright file="MvcExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-15 1:41</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization;
using OSharp.Authorization.Functions;
using OSharp.Dependency;
using OSharp.Exceptions;
using OSharp.Extensions;


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
            return typeInfo.IsClass && !typeInfo.IsAbstract && typeInfo.IsPublic && !typeInfo.ContainsGenericParameters
                && !typeInfo.IsDefined(typeof(NonControllerAttribute)) && (typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                    || typeInfo.IsDefined(typeof(ControllerAttribute)));
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
            IServiceProvider provider = context.HttpContext.RequestServices;
            ScopedDictionary dict = provider.GetService<ScopedDictionary>();
            if (dict.Function != null)
            {
                return dict.Function;
            }
            string area = context.GetAreaName();
            string controller = context.GetControllerName();
            string action = context.GetActionName();
            // todo: 当权限模块没启用时，应取消权限验证，如何判断权限模块已启用？
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
        /// 获取当前Controller中正在操作的Action的相关功能信息
        /// </summary>
        public static IFunction GetExecuteFunction(this ControllerBase controller)
        {
            return controller.ControllerContext.GetExecuteFunction();
        }

        /// <summary>
        /// 获取指定URL的功能信息
        /// </summary>
        public static IFunction GetFunction(this ControllerBase controller, string url)
        {
            url = url.StartsWith("https://") || url.StartsWith("http://")
                ? new Uri(url).AbsolutePath : !url.StartsWith("/") ? $"/{url}" : url;
            IServiceProvider provider = controller.HttpContext.RequestServices;
            IHttpContextFactory factory = provider.GetService<IHttpContextFactory>();
            HttpContext httpContext = factory.Create(controller.HttpContext.Features);
            httpContext.Request.Path = url;
            httpContext.Request.Method = "POST";
            RouteContext routeContext = new RouteContext(httpContext);
            IRouteCollection router = controller.RouteData.Routers.OfType<IRouteCollection>().FirstOrDefault();
            if (router == null)
            {
                return null;
            }
            router.RouteAsync(routeContext).Wait();
            if (routeContext.Handler == null)
            {
                return null;
            }
            RouteValueDictionary dict = routeContext.RouteData.Values;
            string areaName = dict.GetOrDefault("area")?.ToString();
            string controllerName = dict.GetOrDefault("controller")?.ToString();
            string actionName = dict.GetOrDefault("action")?.ToString();
            IFunctionHandler handler = provider.GetService<IFunctionHandler>();
            return handler?.GetFunction(areaName, controllerName, actionName);
        }

        /// <summary>
        /// 检测当前用户是否拥有指定URL的功能权限
        /// </summary>
        public static bool CheckFunctionAuth(this ControllerBase controller, string url)
        {
            IFunction function = controller.GetFunction(url);
            if (function == null)
            {
                return false;
            }
            IFunctionAuthorization authorization = controller.HttpContext.RequestServices.GetService<IFunctionAuthorization>();
            return authorization.Authorize(function, controller.User).IsOk;
        }

        /// <summary>
        /// 检测当前用户是否有指定功能的功能权限
        /// </summary>
        public static bool CheckFunctionAuth(this ControllerBase controller, string actionName, string controllerName, string areaName = null)
        {
            IServiceProvider provider = controller.HttpContext.RequestServices;
            IFunctionHandler functionHandler = provider.GetService<IFunctionHandler>();
            IFunction function = functionHandler?.GetFunction(areaName, controllerName, actionName);
            if (function == null)
            {
                return false;
            }
            IFunctionAuthorization authorization = provider.GetService<IFunctionAuthorization>();
            return authorization.Authorize(function, controller.User).IsOk;
        }
    }
}