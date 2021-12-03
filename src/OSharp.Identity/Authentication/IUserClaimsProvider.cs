﻿// -----------------------------------------------------------------------
//  <copyright file="IUserClaimsProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-02 19:11</last-date>
// -----------------------------------------------------------------------

using System.Security.Claims;
using System.Threading.Tasks;

using OSharp.Data;


namespace OSharp.Authentication
{
    /// <summary>
    /// 定义用户Claims提供器
    /// </summary>
    public interface IUserClaimsProvider
    {
        /// <summary>
        /// 创建用户标识
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Claim[]> CreateClaims(string userId);

        /// <summary>
        /// 请求的Token验证成功后使用OnlineUser信息刷新Identity，将在线用户信息赋予Identity
        /// </summary>
        /// <param name="identity">待刷新的Identity</param>
        /// <returns>刷新后的Identity</returns>
        Task<OperationResult<ClaimsIdentity>> RefreshIdentity(ClaimsIdentity identity);
    }
}