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

using OSharp.Authorization.Functions;
using OSharp.Data;
using OSharp.Identity;


namespace OSharp.Authorization
{
    /// <summary>
    /// 功能权限检查基类
    /// </summary>
    public abstract class FunctionAuthorizationBase : IFunctionAuthorization
    {
        /// <summary>
        /// 初始化一个<see cref="FunctionAuthorizationBase"/>类型的新实例
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
        /// 获取功能权限检查通过的角色
        /// </summary>
        /// <param name="function">要检查的功能</param>
        /// <param name="principal">在线用户信息</param>
        /// <returns>通过的角色</returns>
        public virtual string[] GetOkRoles(IFunction function, IPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return new string[0];
            }

            string[] userRoles = principal.Identity.GetRoles();
            if (function.AccessType != FunctionAccessType.RoleLimit)
            {
                //不是角色限制的功能，允许用户的所有角色
                return userRoles;
            }
            string[] functionRoles = FunctionAuthCache.GetFunctionRoles(function.Id);

            return userRoles.Intersect(functionRoles).ToArray();
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
            if (function.AccessType == FunctionAccessType.Anonymous)
            {
                return AuthorizationResult.OK;
            }
            //未登录
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return new AuthorizationResult(AuthorizationStatus.Unauthorized);
            }
            //已登录，无角色限制
            if (function.AccessType == FunctionAccessType.LoggedIn)
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
            //检查角色-功能的权限
            string[] userRoleNames = identity.GetRoles().ToArray();
            AuthorizationResult result = AuthorizeRoleNames(function, userRoleNames);
            if (result.IsOk)
            {
                return result;
            }
            result = AuthorizeUserName(function, principal.Identity.GetUserName());
            return result;
        }

        /// <summary>
        /// 重写以实现指定角色是否有执行指定功能的权限
        /// </summary>
        /// <param name="function">功能信息</param>
        /// <param name="roleNames">角色名称</param>
        /// <returns>功能权限检查结果</returns>
        protected virtual AuthorizationResult AuthorizeRoleNames(IFunction function, params string[] roleNames)
        {
            Check.NotNull(roleNames, nameof(roleNames));

            if (roleNames.Length == 0)
            {
                return new AuthorizationResult(AuthorizationStatus.Forbidden);
            }
            if (function.AccessType != FunctionAccessType.RoleLimit || roleNames.Contains(SuperRoleName))
            {
                return AuthorizationResult.OK;
            }
            string[] functionRoleNames = FunctionAuthCache.GetFunctionRoles(function.Id);
            if (roleNames.Intersect(functionRoleNames).Any())
            {
                return AuthorizationResult.OK;
            }
            return new AuthorizationResult(AuthorizationStatus.Forbidden);
        }

        /// <summary>
        /// 重写以实现指定用户是否有执行指定功能的权限
        /// </summary>
        /// <param name="function">功能信息</param>
        /// <param name="userName">用户名</param>
        /// <returns>功能权限检查结果</returns>
        protected virtual AuthorizationResult AuthorizeUserName(IFunction function, string userName)
        {
            if (function.AccessType != FunctionAccessType.RoleLimit)
            {
                return AuthorizationResult.OK;
            }

            Guid[] functionIds = FunctionAuthCache.GetUserFunctions(userName);
            if (functionIds.Contains(function.Id))
            {
                return AuthorizationResult.OK;
            }
            return new AuthorizationResult(AuthorizationStatus.Forbidden);
        }
    }
}