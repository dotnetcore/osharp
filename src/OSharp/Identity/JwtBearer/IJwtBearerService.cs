// -----------------------------------------------------------------------
//  <copyright file="IJwtBearerService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-02 1:34</last-date>
// -----------------------------------------------------------------------

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
        /// 创建指定用户的JwtToken信息
        /// </summary>
        /// <param name="userId">用户编号的字符串</param>
        /// <param name="userName">用户名的字符串</param>
        /// <param name="clientId">关联的客户端标识</param>
        /// <returns>JwtToken信息</returns>
        Task<JsonWebToken> CreateToken(string userId, string userName, string clientId = null);

        /// <summary>
        /// 使用RefreshToken获取新的JwtToken信息
        /// </summary>
        /// <param name="refreshToken">刷新Token字符串</param>
        /// <returns>JwtToken信息</returns>
        Task<JsonWebToken> RefreshToken(string refreshToken);
    }
}