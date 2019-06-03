// -----------------------------------------------------------------------
//  <copyright file="JwtTokenType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-02 2:02</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// JwtToken类型
    /// </summary>
    public enum JwtTokenType
    {
        /// <summary>
        /// 刷新Token
        /// </summary>
        RefreshToken,

        /// <summary>
        /// 访问Token
        /// </summary>
        AccessToken
    }
}