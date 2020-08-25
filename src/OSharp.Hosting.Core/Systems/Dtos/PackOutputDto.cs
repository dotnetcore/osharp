// -----------------------------------------------------------------------
//  <copyright file="PackOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-13 14:59</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Core.Packs;
using OSharp.Entity;


namespace OSharp.Hosting.Systems.Dtos
{
    /// <summary>
    /// 输出DTO：模块包信息
    /// </summary>
    public class PackOutputDto : IOutputDto
    {
        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        [DisplayName("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 显示名称
        /// </summary>
        [DisplayName("显示名称")]
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 类型路径
        /// </summary>
        [DisplayName("类型路径")]
        public string Class { get; set; }

        /// <summary>
        /// 获取或设置 模块级别
        /// </summary>
        [DisplayName("级别")]
        public PackLevel Level { get; set; }

        /// <summary>
        /// 获取或设置 启动顺序
        /// </summary>
        [DisplayName("启动顺序")]
        public int Order { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        [DisplayName("是否启用")]
        public bool IsEnabled { get; set; }
    }
}