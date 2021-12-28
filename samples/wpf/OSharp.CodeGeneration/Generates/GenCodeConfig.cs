// -----------------------------------------------------------------------
//  <copyright file="CodeConfig.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 21:39</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Extensions;


namespace OSharp.CodeGeneration.Generates
{
    /// <summary>
    /// 代码配置
    /// </summary>
    public class GenCodeConfig
    {
        /// <summary>
        /// 获取或设置 配置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 元数据类型
        /// </summary>
        public MetadataType MetadataType { get; set; }

        /// <summary>
        /// 获取或设置 模板文件，默认内置，也可以由用户自定义加载
        /// </summary>
        public string TemplateFile { get; set; }

        /// <summary>
        /// 获取或设置 模板类型，内置模板使用
        /// </summary>
        public Type TemplateType { get; set; }

        /// <summary>
        /// 获取或设置 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 获取或设置 代码输出文件名格式
        /// </summary>
        public string OutputFileFormat { get; set; }

        /// <summary>
        /// 获取或设置 是否只生成一次
        /// </summary>
        public bool IsOnce { get; set; }

        /// <summary>
        /// 获取或设置 系统类型
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 获取或设置 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 获取项目代码输出文件名
        /// </summary>
        public string GetCodeFileName(CodeProject project)
        {
            string fileName = OutputFileFormat.Replace("{Project.NamespacePrefix}", project.NamespacePrefix)
                .Replace("{Project.Name:Lower}", project.Name.UpperToLowerAndSplit())
                .Replace("{Project.Name}", project.Name);
            return fileName;
        }

        /// <summary>
        /// 获取项目代码输出文件名
        /// </summary>
        public string GetCodeFileName(CodeModule module)
        {
            string fileName = OutputFileFormat.Replace("{Project.NamespacePrefix}", module.Project.NamespacePrefix)
                .Replace("{Module.Name:Lower}", module.Name.UpperToLowerAndSplit())
                .Replace("{Module.Name}", module.Name);
            return fileName;
        }

        /// <summary>
        /// 获取项目代码输出文件名
        /// </summary>
        public string GetCodeFileName(CodeEntity entity)
        {
            string fileName = OutputFileFormat.Replace("{Project.NamespacePrefix}", entity.Module.Project.NamespacePrefix)
                .Replace("{Module.Name:Lower}", entity.Module.Name.UpperToLowerAndSplit())
                .Replace("{Entity.Name:Lower}", entity.Name.UpperToLowerAndSplit())
                .Replace("{Module.Name}", entity.Module.Name).Replace("{Entity.Name}", entity.Name);
            return fileName;
        }
    }
}
