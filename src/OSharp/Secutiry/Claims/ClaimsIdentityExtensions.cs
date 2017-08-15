// -----------------------------------------------------------------------
//  <copyright file="ClaimsIdentityExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:40</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using System.Security.Claims;


namespace OSharp.Secutiry.Claims
{
    /// <summary>
    /// <see cref="ClaimsIdentity"/>扩展操作类
    /// </summary>
    public static class ClaimsIdentityExtensions
    {
        /// <summary>
        /// 获取指定类型的Claim值
        /// </summary>
        public static string GetClaimValueFirstOrDefault(this ClaimsIdentity identity, string type)
        {
            Claim claim = identity.Claims.FirstOrDefault(m => m.Type == type);
            return claim?.Value;
        }

        /// <summary>
        /// 获取指定类型的所有Claim值
        /// </summary>
        public static string[] GetClaimValues(this ClaimsIdentity identity, string type)
        {
            return identity.Claims.Where(m => m.Type == type).Select(m => m.Value).ToArray();
        }
    }
}