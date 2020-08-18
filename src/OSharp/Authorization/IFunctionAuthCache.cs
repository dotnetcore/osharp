// -----------------------------------------------------------------------
//  <copyright file="IFunctionAuthCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-10 20:03</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Authorization
{
    /// <summary>
    /// 定义功能权限缓存的功能
    /// </summary>
    public interface IFunctionAuthCache
    {
        /// <summary>
        /// 创建功能角色权限缓存，只创建 功能-角色集合 的映射，用户-功能 的映射，遇到才即时创建并缓存
        /// </summary>
        void BuildRoleCaches();

        /// <summary>
        /// 移除指定功能的缓存
        /// </summary>
        /// <param name="functionIds">功能编号集合</param>
        void RemoveFunctionCaches(params Guid[] functionIds);

        /// <summary>
        /// 移除指定用户的缓存
        /// </summary>
        /// <param name="userNames">用户编号集合</param>
        void RemoveUserCaches(params string[] userNames);

        /// <summary>
        /// 获取能执行指定功能的所有角色
        /// </summary>
        /// <param name="functionId">功能编号</param>
        /// <param name="scopeProvider">局部服务提供者</param>
        /// <returns>能执行功能的角色名称集合</returns>
        string[] GetFunctionRoles(Guid functionId, IServiceProvider scopeProvider = null);

        /// <summary>
        /// 获取指定用户的所有特权功能
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户的所有特权功能</returns>
        Guid[] GetUserFunctions(string userName);
    }
}