// -----------------------------------------------------------------------
//  <copyright file="ICodeGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-06 20:12</last-date>
// -----------------------------------------------------------------------

namespace OSharp.CodeGeneration
{
    /// <summary>
    /// 定义代码生成器
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="context">生成代码上下文</param>
        /// <returns>生成的代码字符串</returns>
        string Generate(GenerateContext context);
    }
}