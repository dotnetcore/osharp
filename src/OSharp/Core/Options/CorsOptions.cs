// -----------------------------------------------------------------------
//  <copyright file="CorsOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-12-13 12:37</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Options
{
    /// <summary>
    /// Cors选项
    /// </summary>
    public class CorsOptions
    {
        /// <summary>
        /// 获取或设置 策略名称
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// 获取或设置 允许任意请求头
        /// </summary>
        public bool AllowAnyHeader { get; set; }

        /// <summary>
        /// 获取或设置 允许的请求头
        /// </summary>
        public string[] WithHeaders { get; set; }

        /// <summary>
        /// 获取或设置 允许任意方法
        /// </summary>
        public bool AllowAnyMethod { get; set; }

        /// <summary>
        /// 获取或设置 允许的方法
        /// </summary>
        public string[] WithMethods { get; set; }

        /// <summary>
        /// 获取或设置 允许跨域凭据
        /// </summary>
        public bool AllowCredentials { get; set; }

        /// <summary>
        /// 获取或设置 禁止跨域凭据
        /// </summary>
        public bool DisallowCredentials { get; set; }

        /// <summary>
        /// 获取或设置 允许任意来源
        /// </summary>
        public bool AllowAnyOrigin { get; set; }

        /// <summary>
        /// 获取或设置 允许的来源
        /// </summary>
        public string[] WithOrigins { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}