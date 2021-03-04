// -----------------------------------------------------------------------
//  <copyright file="MvcFunctionAuthorizationHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-11 14:12</last-date>
// -----------------------------------------------------------------------

using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization.Functions;


namespace OSharp.Authorization
{
    /// <summary>
    /// 功能授权处理器
    /// </summary>
    public class FunctionAuthorizationHandler : AuthorizationHandler<FunctionRequirement>
    {
        private readonly IFunctionAuthorization _functionAuthorization;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个<see cref="FunctionAuthorizationHandler"/>类型的新实例
        /// </summary>
        public FunctionAuthorizationHandler(IFunctionAuthorization functionAuthorization, IHttpContextAccessor httpContextAccessor, IServiceProvider provider)
        {
            _functionAuthorization = functionAuthorization;
            _httpContextAccessor = httpContextAccessor;
            _provider = provider;
        }

        /// <summary>
        /// 如果根据特定要求允许授权，则作出决定。
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FunctionRequirement requirement)
        {
            RouteEndpoint endpoint = null;
            HttpContext httpContext = null;
            switch (context.Resource)
            {
                case HttpContext resource1:
                    httpContext = resource1;
                    endpoint = httpContext.GetEndpoint() as RouteEndpoint;
                    break;
                case RouteEndpoint resource2:
                    httpContext = _httpContextAccessor.HttpContext;
                    endpoint = resource2;
                    break;
            }

            if (endpoint == null || httpContext == null)
            {
                return Task.CompletedTask;
            }

            IFunction function = endpoint.GetExecuteFunction(httpContext);
            AuthorizationResult result = AuthorizeCore(context, function);
            if (result.IsOk)
            {
                context.Succeed(requirement);
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
    }
}