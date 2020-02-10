// -----------------------------------------------------------------------
//  <copyright file="AuthorizationResultType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-10 19:37</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;


namespace OSharp.Authorization
{
    /// <summary>
    /// 权限检查结果状态
    /// </summary>
    public enum AuthorizationStatus
    {
        /// <summary>
        /// 权限检查通过
        /// </summary>
        [Description("权限检查通过")] OK = 200,

        /// <summary>
        /// 未登录而被拒绝
        /// </summary>
        [Description("该操作需要登录后才能继续进行")] Unauthorized = 401,

        /// <summary>
        /// 已登录，但权限不足
        /// </summary>
        [Description("当前用户权限不足，不能继续执行")] Forbidden = 403,

        /// <summary>
        /// 找不到指定资源
        /// </summary>
        [Description("指定的功能不存在")] NoFound = 404,

        /// <summary>
        /// 资源被锁定
        /// </summary>
        [Description("指定的功能被锁定")] Locked = 423,

        /// <summary>
        /// 权限检查出现错误
        /// </summary>
        [Description("权限检测出现错误")] Error = 500
    }
}