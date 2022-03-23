// -----------------------------------------------------------------------
//  <copyright file="ICodeGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 21:39</last-date>
// -----------------------------------------------------------------------

using System.Threading.Tasks;

using OSharp.CodeGeneration.Services.Entities;


namespace OSharp.CodeGeneration.Generates
{
    /// <summary>
    /// 定义代码生成器
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// 生成项目所有代码
        /// </summary>
        /// <param name="templates">要处理的代码模板集合</param>
        /// <param name="project">代码项目信息</param>
        /// <returns>输出的代码文件信息集合</returns>
        Task<CodeFile[]> GenerateCodes(CodeTemplate[] templates, CodeProject project);

        /// <summary>
        /// 生成项目元数据相关代码
        /// <param name="template">代码模板</param>
        /// <param name="project">项目元数据</param>
        /// </summary>
        Task<CodeFile> GenerateCode(CodeTemplate template, CodeProject project);

        /// <summary>
        /// 生成模块元数据相关代码
        /// <param name="template">代码模板</param>
        /// <param name="module">模块元数据</param>
        /// </summary>
        Task<CodeFile> GenerateCode(CodeTemplate template, CodeModule module);

        /// <summary>
        /// 生成实体元数据相关代码
        /// <param name="template">代码模板</param>
        /// <param name="entity">实体元数据</param>
        /// </summary>
        Task<CodeFile> GenerateCode(CodeTemplate template, CodeEntity entity);
    }
}
