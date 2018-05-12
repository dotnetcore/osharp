// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorizeAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-12 10:43</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Net;
using System.Security.Principal;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

using OSharp.AspNetCore.Http;
using OSharp.AspNetCore.UI;
using OSharp.Core.Functions;
using OSharp.Secutiry;

using AuthorizationResult = OSharp.Secutiry.AuthorizationResult;


namespace OSharp.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 功能权限授权验证
    /// </summary>
    public class FunctionAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Check.NotNull(context, nameof(context));

            if (!(context.ActionDescriptor is ControllerActionDescriptor descriptor))
            {
                return;
            }
            //Action允许匿名，直接允许
            if (descriptor.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }
            //Controller允许匿名，Action未显式需要权限
            if (descriptor.ControllerTypeInfo.IsDefined(typeof(AllowAnonymousAttribute), false)
                && !descriptor.MethodInfo.IsDefined(typeof(AuthorizeAttribute), true)
                && !descriptor.MethodInfo.IsDefined(typeof(FunctionAuthorizeAttribute), true))
            {
                return;
            }
            IFunction function = context.GetExecuteFunction();
            AuthorizationResult result = AuthorizeCore(context.HttpContext, function);
            if (!result.IsOk)
            {
                HandleUnauthorizedRequest(context, result);
            }
        }

        /// <summary>
        /// 重写以实现功能权限的核心验证逻辑
        /// </summary>
        /// <param name="httpContext">Http请求上下文</param>
        /// <param name="function">要验证的功能</param>
        /// <returns>权限验证结果</returns>
        protected virtual AuthorizationResult AuthorizeCore(HttpContext httpContext, IFunction function)
        {
            IPrincipal user = httpContext.User;
            IFunctionAuthorization authorization = ServiceLocator.Instance.GetService<IFunctionAuthorization>();
            AuthorizationResult result = authorization.Authorize(function, user);
            return result;
        }

        /// <summary>
        /// 重写以实现授权未通过的处理逻辑
        /// </summary>
        /// <param name="context">权限验证上下文</param>
        /// <param name="result">权限检查结果</param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationFilterContext context, AuthorizationResult result)
        {
            //Json方式请求，返回AjaxResult
            bool isJsRequest = context.HttpContext.Request.IsAjaxRequest() || context.HttpContext.Request.IsJsonContextType();

            AuthorizationStatus status = result.ResultType;
            switch (status)
            {
                case AuthorizationStatus.Unauthorized:
                    context.Result = isJsRequest
                        ? (IActionResult)new JsonResult(new AjaxResult(result.Message, AjaxResultType.UnAuth))
                        : new UnauthorizedResult();
                    break;
                case AuthorizationStatus.Forbidden:
                    context.Result = isJsRequest
                        ? (IActionResult)new JsonResult(new AjaxResult(result.Message, AjaxResultType.Forbidden))
                        : new StatusCodeResult(403);
                    break;
                case AuthorizationStatus.NoFound:
                    context.Result = isJsRequest
                        ? (IActionResult)new JsonResult(new AjaxResult(result.Message, AjaxResultType.NoFound))
                        : new StatusCodeResult(404);
                    break;
                case AuthorizationStatus.Locked:
                    context.Result = isJsRequest
                        ? (IActionResult)new JsonResult(new AjaxResult(result.Message, AjaxResultType.Locked))
                        : new StatusCodeResult(423);
                    break;
                case AuthorizationStatus.Error:
                    context.Result = isJsRequest
                        ? (IActionResult)new JsonResult(new AjaxResult(result.Message, AjaxResultType.Error))
                        : new StatusCodeResult(500);
                    break;
            }
            if (isJsRequest)
            {
                context.HttpContext.Response.StatusCode = 200;
            }
        }

    }
}