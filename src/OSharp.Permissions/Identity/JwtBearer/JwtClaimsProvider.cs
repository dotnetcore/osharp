// -----------------------------------------------------------------------
//  <copyright file="JwtClaimsProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-09 9:45</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// Jwt标识提供器
    /// </summary>
    /// <typeparam name="TUser">用户类型</typeparam>
    /// <typeparam name="TUserKey">用户编号类型</typeparam>
    public class JwtClaimsProvider<TUser, TUserKey> : IJwtClaimsProvider<TUser>
        where TUser : UserBase<TUserKey>
        where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// 创建指定用户的标识
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public virtual Task<Claim[]> Create(TUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            return Task.FromResult(claims.ToArray());
        }
    }
}