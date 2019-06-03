// -----------------------------------------------------------------------
//  <copyright file="IJwtBearerService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-02 1:34</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Security.Claims;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// 定义JwtBearer服务，负责JwtToken的创建，验证，刷新等业务
    /// </summary>
    public interface IJwtBearerService
    {
        /// <summary>
        /// 创建RefreshToken
        /// </summary>
        /// <param name="claims">要加入Token的声明信息</param>
        /// <returns></returns>
        string CreateRefreshToken(IList<Claim> claims);

        /// <summary>
        /// 使用RefreshToken创建AccessToken
        /// </summary>
        /// <param name="claims">要加入Token的声明信息</param>
        /// <param name="refreshToken">刷新Token</param>
        /// <returns></returns>
        string CreateAccessToken(IList<Claim> claims, string refreshToken);

        /// <summary>
        /// 验证指定的Token有效性
        /// </summary>
        /// <param name="token">指定Token</param>
        /// <returns></returns>
        ClaimsPrincipal ValidateToken(string token);
    }
}