// -----------------------------------------------------------------------
//  <copyright file="GrantType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-12 0:14</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// 授权类型
    /// </summary>
    public class GrantType
    {
        /// <summary>
        /// 用户密码类型
        /// </summary>
        public const string Password = "password";

        /// <summary>
        /// 刷新Token类型
        /// </summary>
        public const string RefreshToken = "refresh_token";
    }
}