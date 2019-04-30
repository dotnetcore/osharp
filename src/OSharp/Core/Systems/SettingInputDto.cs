// -----------------------------------------------------------------------
//  <copyright file="SettingInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-24 17:21</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;


namespace OSharp.Core.Systems
{
    /// <summary>
    /// 设置信息输入DTO
    /// </summary>
    public class SettingInputDto
    {
        /// <summary>
        /// 获取或设置 设置类型全名
        /// </summary>
        [Required]
        public string SettingTypeName { get; set; }

        /// <summary>
        /// 获取或设置 设置模型JSON
        /// </summary>
        public string SettingJson { get; set; }
    }
}