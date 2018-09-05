// -----------------------------------------------------------------------
//  <copyright file="SettingOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-24 17:26</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Core.Systems
{
    /// <summary>
    /// 设置输出DTO
    /// </summary>
    public class SettingOutputDto
    {
        /// <summary>
        /// 初始化一个<see cref="SettingOutputDto"/>类型的新实例
        /// </summary>
        public SettingOutputDto(ISetting setting)
        {
            Setting = setting;
            SettingTypeName = setting.GetType().GetFullNameWithModule();
        }

        /// <summary>
        /// 获取 设置类型全名
        /// </summary>
        public string SettingTypeName { get; }

        /// <summary>
        /// 获取 设置信息
        /// </summary>
        public ISetting Setting { get; }
    }
}