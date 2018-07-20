// -----------------------------------------------------------------------
//  <copyright file="JwtClaimTypes.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-26 0:07</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Identity.JwtBearer
{
    /// <summary>
    /// Jwt声明类型
    /// </summary>
    public struct JwtClaimTypes
    {
        /// <summary>
        /// 是否管理
        /// </summary>
        public const string IsAdmin = "is-admin";
        /// <summary>
        /// 头像图片
        /// </summary>
        public const string HeadImage = "headimg";
    }
}