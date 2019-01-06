// -----------------------------------------------------------------------
//  <copyright file="ProjectMetadata.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-07 0:21</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace OSharp.CodeGeneration.Schema
{
    /// <summary>
    /// 项目元数据信息
    /// </summary>
    public class ProjectMetadata
    {
        /// <summary>
        /// 获取或设置 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 获取或设置 项目显示名称
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 命名空间前缀，通常形如“公司.项目”
        /// </summary>
        public string NamespacePrefix { get; set; }

        /// <summary>
        /// 获取或设置 站点地址
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// 获取或设置 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 获取或设置 Copyright
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// 获取或设置 模块信息集合
        /// </summary>
        public ICollection<ModuleMetadata> Modules { get; set; } = new List<ModuleMetadata>();
    }
}