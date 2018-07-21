// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorizationBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-10 20:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

using OSharp.Core.Functions;
using OSharp.Entity;
using OSharp.Secutiry.Claims;


namespace OSharp.Secutiry
{
    /// <summary>
    /// 功能权限检查基类
    /// </summary>
    public abstract class FunctionAuthorizationBase<TFunction> : IFunctionAuthorization
        where TFunction : class, IFunction, IEntity<Guid>
    {
        /// <summary>
        /// 初始化一个<see cref="FunctionAuthorizationBase{TFunction}"/>类型的新实例
        /// </summary>
        protected FunctionAuthorizationBase(IFunctionAuthCache functionAuthCache)
        {
            FunctionAuthCache = functionAuthCache;
            SuperRoleName = "系统管理员";
        }

        /// <summary>
        /// 获取 功能权限缓存
        /// </summary>
        protected IFunctionAuthCache FunctionAuthCache { get; }

        /// <summary>
        /// 获取 超级管理员角色
        /// </summary>
        protected virtual string SuperRoleName { get; }

        /// <summary>
        /// 检查指定用户是否有执行指定功能的权限
        /// </summary>
        /// <param name="function">要检查的功能</param>
        /// <param name="principal">在线用户信息</param>
        /// <returns>功能权限检查结果</returns>
        public AuthorizationResult Authorize(IFunction function, IPrincipal principal)
        {
            return AuthorizeCore(function, principal);
        }

        /// <summary>
        /// 重写以实现权限检查核心验证操作
        /// </summary>
        /// <param name="function">要验证的功能信息</param>
        /// <param name="principal">当前用户在线信息</param>
        /// <returns>功能权限验证结果</returns>
        protected virtual AuthorizationResult AuthorizeCore(IFunction function, IPrincipal principal)
        {
            if (function == null)
            {
                return new AuthorizationResult(AuthorizationStatus.NoFound);
            }
            if (function.IsLocked)
            {
                return new AuthorizationResult(AuthorizationStatus.Locked, $"功能“{function.Name}”已被禁用，无法执行");
            }
            if (function.AccessType == FunctionAccessType.Anonymouse)
            {
                return AuthorizationResult.OK;
            }
            //未登录
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return new AuthorizationResult(AuthorizationStatus.Unauthorized);
            }
            //已登录，无角色限制
            if (function.AccessType == FunctionAccessType.Logined)
            {
                return AuthorizationResult.OK;
            }
            return AuthorizeRoleLimit(function, principal);
        }

        /// <summary>
        /// 重写以实现 角色限制 的功能的功能权限检查
        /// </summary>
        /// <param name="function">要验证的功能信息</param>
        /// <param name="principal">用户在线信息</param>
        /// <returns>功能权限验证结果</returns>
        protected virtual AuthorizationResult AuthorizeRoleLimit(IFunction function, IPrincipal principal)
        {
            //角色限制
            if (!(principal.Identity is ClaimsIdentity identity))
            {
                return new AuthorizationResult(AuthorizationStatus.Error, "当前用户标识IIdentity格式不正确，仅支持ClaimsIdentity类型的用户标识");
            }
            if (!(function is TFunction func))
            {
                return new AuthorizationResult(AuthorizationStatus.Error, $"要检测的功能类型为“{function.GetType()}”，不是要求的“{typeof(TFunction)}”类型");
            }
            //检查角色-功能的权限
            string[] userRoleNames = identity.GetRoles().ToArray();
            //如果是超级管理员角色，直接通过
            if (userRoleNames.Contains(SuperRoleName))
            {
                return AuthorizationResult.OK;
            }

            string[] functionRoleNames = FunctionAuthCache.GetFunctionRoles(func.Id);
            if (userRoleNames.Intersect(functionRoleNames).Any())
            {
                return AuthorizationResult.OK;
            }
            //检查用户-功能的权限
            Guid[] functionIds = FunctionAuthCache.GetUserFunctions(identity.GetUserName());
            if (functionIds.Contains(func.Id))
            {
                return AuthorizationResult.OK;
            }
            return new AuthorizationResult(AuthorizationStatus.Forbidden);
        }
    }
}