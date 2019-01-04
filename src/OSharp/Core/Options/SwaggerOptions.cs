// -----------------------------------------------------------------------
//  <copyright file="SwaggerOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-14 23:28</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Options
{
    /// <summary>
    /// Swagger选项
    /// </summary>
    public class SwaggerOptions
    {
        /// <summary>
        /// 获取或设置 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置 版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 获取或设置 Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}