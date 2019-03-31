// -----------------------------------------------------------------------
//  <copyright file="OAuth2Options.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-31 18:18</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Options
{
    /// <summary>
    /// 第三方OAuth2登录的配置选项
    /// </summary>
    public class OAuth2Options
    {
        /// <summary>
        /// 获取或设置 本应用在第三方OAuth2系统中的客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 获取或设置 本应用在第三方OAuth2系统中的客户端密钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}