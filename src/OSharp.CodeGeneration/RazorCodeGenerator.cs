// -----------------------------------------------------------------------
//  <copyright file="RazorCodeGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-06 22:23</last-date>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;

using OSharp.CodeGeneration.Schema;
using OSharp.Exceptions;

using RazorEngine;
using RazorEngine.Templating;


namespace OSharp.CodeGeneration
{
    /// <summary>
    /// Razor代码生成器
    /// </summary>
    public class RazorCodeGenerator : ICodeGenerator
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="context">生成代码上下文</param>
        /// <returns>生成的代码字符串</returns>
        public virtual string Generate(GenerateContext context)
        {
            


            throw new NotImplementedException();
        }

        /// <summary>
        /// 由实体元数据生成实体类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns></returns>
        protected virtual string GenerateEntityCode(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.Entity);
            return Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
        }

        /// <summary>
        /// 由模块元数据生成模块业务契约接口代码
        /// </summary>
        /// <param name="module">模块元数据</param>
        /// <returns></returns>
        protected virtual string GenerateServiceContract(ModuleMetadata module)
        {
            string template = GetInternalTemplate(CodeType.ServiceContract);
            return Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), module.GetType(), module);
        }

        /// <summary>
        /// 由模块元数据生成模块业务综合实现类代码
        /// </summary>
        /// <param name="module">模块元数据</param>
        /// <returns></returns>
        protected virtual string GenerateServiceMainImpl(ModuleMetadata module)
        {
            string template = GetInternalTemplate(CodeType.ServiceMainImpl);
            return Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), module.GetType(), module);
        }

        /// <summary>
        /// 由模块元数据生成模块业务单实体实现类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns></returns>
        protected virtual string GenerateServiceEntityImpl(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.ServiceEntityImpl);
            return Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
        }

        /// <summary>
        /// 由模块元数据生成实体数据映射配置类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns></returns>
        protected virtual string GenerateEntityConfiguration(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.EntityConfiguration);
            return Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
        }

        /// <summary>
        /// 由模块元数据生成实体控制器类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns></returns>
        protected virtual string GenerateController(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.EntityController);
            return Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
        }
        
        /// <summary>
        /// 读取指定代码类型的内置代码模板
        /// </summary>
        /// <param name="type">代码类型</param>
        /// <returns></returns>
        private string GetInternalTemplate(CodeType type)
        {
            string resName = $"OSharp.CodeGeneration.Templates.{type.ToString()}.cshtml";
            Stream stream = GetType().Assembly.GetManifestResourceStream(resName);
            if (stream == null)
            {
                throw new OsharpException($"无法找到“{type.ToString()}”的内置代码模板");
            }
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}