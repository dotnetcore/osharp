// -----------------------------------------------------------------------
//  <copyright file="MvcFunctionAuthorizationHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-11 14:12</last-date>
// -----------------------------------------------------------------------

using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using OSharp.AspNetCore;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Functions;
using OSharp.Data;


namespace OSharp.Authorization
{
    /// <summary>
    /// MVC功能授权处理器
    /// </summary>
    public class FunctionAuthorizationHandler : AuthorizationHandler<FunctionRequirement>
    {
        private readonly IFunctionAuthorization _functionAuthorization;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 初始化一个<see cref="FunctionAuthorizationHandler"/>类型的新实例
        /// </summary>
        public FunctionAuthorizationHandler(IFunctionAuthorization functionAuthorization, IHttpContextAccessor httpContextAccessor)
        {
            _functionAuthorization = functionAuthorization;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 如果根据特定要求允许授权，则作出决定。
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FunctionRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (context.Resource is RouteEndpoint endpoint)
            {
                IFunction function = endpoint.GetExecuteFunction(httpContext);
                AuthorizationResult result = AuthorizeCore(context, function);
                if (result.IsOk)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 重写以实现功能权限的核心验证逻辑
        /// </summary>
        /// <param name="context">权限过滤器上下文</param>
        /// <param name="function">要验证的功能</param>
        /// <returns>权限验证结果</returns>
        protected virtual AuthorizationResult AuthorizeCore(AuthorizationHandlerContext context, IFunction function)
        {
            ClaimsPrincipal user = context.User;
            AuthorizationResult result = _functionAuthorization.Authorize(function, user);
            return result;
        }

        /// <summary>
        /// 重写以实现授权未通过的处理逻辑
        /// </summary>
        /// <param name="context">权限验证上下文</param>
        /// <param name="result">权限检查结果</param>
        protected virtual void HandleMvcUnauthorizedRequest(AuthorizationFilterContext context, AuthorizationResult result)
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