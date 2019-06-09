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
using System.Threading.Tasks;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// 定义JwtBearer服务，负责JwtToken的创建，验证，刷新等业务
    /// </summary>
    public interface IJwtBearerService
    {
        /// <summary>
        /// 创建指定用户的AccessToken
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        Task<string> CreateAccessToken(string userId);

        /// <summary>
        /// 使用AccessToken刷新一个新的AccessToken，如果AccessToken有效，原样返回，否则返回一个新的
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <returns>AccessToken</returns>
        Task<string> RefreshAccessToken(string accessToken);
    }
}