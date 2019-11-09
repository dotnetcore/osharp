// -----------------------------------------------------------------------
//  <copyright file="JsonWebToken.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-12 15:31</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Extensions;
using OSharp.Timing;


namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// JwtToken模型
    /// </summary>
    public class JsonWebToken
    {
        /// <summary>
        /// 获取或设置 用于业务身份认证的AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 获取或设置 用于刷新AccessToken的RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 获取或设置 RefreshToken有效期，UTC标准
        /// </summary>
        public long RefreshUctExpires { get; set; }

        /// <summary>
        /// 刷新Token是否过期
        /// </summary>
        public bool IsRefreshExpired()
        {
            DateTime now = DateTime.Now;
            long nowTick = now.ToJsGetTime().CastTo<long>(0);
            return RefreshUctExpires > nowTick;
        }
    }
}