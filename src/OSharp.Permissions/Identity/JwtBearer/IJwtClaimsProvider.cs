// -----------------------------------------------------------------------
//  <copyright file="IJwtClaimsProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-09 9:43</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// 定义Jwt标识提供器，获取指定用户的标识用于创建Jwt-Token
    /// </summary>
    public interface IJwtClaimsProvider<in TUser>
        where TUser : class
    {
        /// <summary>
        /// 创建指定用户的标识
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Claim[]> Create(TUser user);
    }
}