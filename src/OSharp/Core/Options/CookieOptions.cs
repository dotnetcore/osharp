// -----------------------------------------------------------------------
//  <copyright file="CookieOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-13 22:36</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Options
{
    /// <summary>
    /// Cookie身份认证选项
    /// </summary>
    public class CookieOptions
    {
        /// <summary>
        /// 获取或设置 Cookie名称
        /// </summary>
        public string CookieName { get; set; }

        /// <summary>
        /// 获取或设置 登录地址
        /// </summary>
        public string LoginPath { get; set; }

        /// <summary>
        /// 获取或设置 登出地址
        /// </summary>
        public string LogoutPath { get; set; }

        /// <summary>
        /// 获取或设置 无权限地址
        /// </summary>
        public string AccessDeniedPath { get; set; }

        /// <summary>
        /// 获取或设置 返回Url参数名，默认为 ReturnUrl
        /// </summary>
        public string ReturnUrlParameter { get; set; }

        /// <summary>
        /// 获取或设置 过期分钟数
        /// </summary>
        public int ExpireMins { get; set; }

        /// <summary>
        /// 获取或设置 是否滑动过期
        /// </summary>
        public bool SlidingExpiration { get; set; } = true;

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
