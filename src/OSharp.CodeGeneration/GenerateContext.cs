// -----------------------------------------------------------------------
//  <copyright file="GenerateContext.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-06 20:15</last-date>
// -----------------------------------------------------------------------

using OSharp.CodeGeneration.Schema;


namespace OSharp.CodeGeneration
{
    /// <summary>
    /// 生成代码环境上下文
    /// </summary>
    public class GenerateContext
    {
        /// <summary>
        /// 获取或设置 实体类型元数据
        /// </summary>
        public EntityMetadata[] EntityMetadatas { get; set; } = new EntityMetadata[0];

        /// <summary>
        /// 获取或设置 项目信息
        /// </summary>
        public ProjectMetadata Project { get; set; }

        /// <summary>
        /// 获取或设置 模块信息
        /// </summary>
        public ModuleMetadata Module { get; set; }

        /// <summary>
        /// 获取或设置 代码模板字符串
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// 获取或设置 代码类型
        /// </summary>
        public CodeType CodeType { get; set; } = CodeType.Entity;
    }
}