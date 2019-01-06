// -----------------------------------------------------------------------
//  <copyright file="RazorCodeGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-07 2:31</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        /// 生成项目文件
        /// </summary>
        /// <param name="project">项目元数据</param>
        /// <returns>项目代码</returns>
        public virtual CodeFile[] GenerateProjectCode(ProjectMetadata project)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            ModuleMetadata[] modules = project.Modules.ToArray();
            EntityMetadata[] entities = modules.SelectMany(m => m.Entities).ToArray();

            codeFiles.AddRange(entities.Select(GenerateEntityCode));
            codeFiles.AddRange(entities.Select(GenerateInputDtoCode));
            codeFiles.AddRange(entities.Select(GenerateOutputDtoCode));
            codeFiles.AddRange(entities.Select(GenerateServiceEntityImpl));
            codeFiles.AddRange(entities.Select(GenerateEntityConfiguration));
            codeFiles.AddRange(entities.Select(GenerateAdminController));

            codeFiles.AddRange(modules.Select(GenerateServiceContract));
            codeFiles.AddRange(modules.Select(GenerateServiceMainImpl));

            return codeFiles.OrderBy(m => m.FileName).ToArray();
        }

        /// <summary>
        /// 由实体元数据生成实体类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>实体类代码</returns>
        public virtual CodeFile GenerateEntityCode(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.Entity);
            string code = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
            return new CodeFile()
            {
                SourceCode = code,
                FileName = $"{entity.Module.Project.NamespacePrefix}.Core/{entity.Module.Name}/Entities/{entity.Name}.cs"
            };
        }

        /// <summary>
        /// 由实体元数据生成输入DTO类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>输入DTO类代码</returns>
        public virtual CodeFile GenerateInputDtoCode(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.InputDto);
            string code = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
            return new CodeFile()
            {
                SourceCode = code,
                FileName = $"{entity.Module.Project.NamespacePrefix}.Core/{entity.Module.Name}/Dtos/{entity.Name}InputDto.cs"
            };
        }

        /// <summary>
        /// 由实体元数据生成输出DTO类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>输出DTO类代码</returns>
        public virtual CodeFile GenerateOutputDtoCode(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.OutputDto);
            string code = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
            return new CodeFile()
            {
                SourceCode = code,
                FileName = $"{entity.Module.Project.NamespacePrefix}.Core/{entity.Module.Name}/Dtos/{entity.Name}OutputDto.cs"
            };
        }

        /// <summary>
        /// 由模块元数据生成模块业务契约接口代码
        /// </summary>
        /// <param name="module">模块元数据</param>
        /// <returns>模块业务契约接口代码</returns>
        public virtual CodeFile GenerateServiceContract(ModuleMetadata module)
        {
            string template = GetInternalTemplate(CodeType.ServiceContract);
            string code = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), module.GetType(), module);
            return new CodeFile()
            {
                SourceCode = code,
                FileName = $"{module.Project.NamespacePrefix}.Core/{module.Name}/I{module.Name}Contract.cs"
            };
        }

        /// <summary>
        /// 由模块元数据生成模块业务综合实现类代码
        /// </summary>
        /// <param name="module">模块元数据</param>
        /// <returns>模块业务综合实现类代码</returns>
        public virtual CodeFile GenerateServiceMainImpl(ModuleMetadata module)
        {
            string template = GetInternalTemplate(CodeType.ServiceMainImpl);
            string code = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), module.GetType(), module);
            return new CodeFile()
            {
                SourceCode = code,
                FileName = $"{module.Project.NamespacePrefix}.Core/{module.Name}/{module.Name}Service.cs"
            };
        }

        /// <summary>
        /// 由模块元数据生成模块业务单实体实现类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>模块业务单实体实现类代码</returns>
        public virtual CodeFile GenerateServiceEntityImpl(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.ServiceEntityImpl);
            string code = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
            return new CodeFile()
            {
                SourceCode = code,
                FileName = $"{entity.Module.Project.NamespacePrefix}.Core/{entity.Module.Name}/{entity.Module.Name}Service.{entity.Name}.cs"
            };
        }

        /// <summary>
        /// 由模块元数据生成实体数据映射配置类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>实体数据映射配置类代码</returns>
        public virtual CodeFile GenerateEntityConfiguration(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.EntityConfiguration);
            string code = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
            return new CodeFile()
            {
                SourceCode = code,
                FileName = $"{entity.Module.Project.NamespacePrefix}.EntityConfiguration/{entity.Module.Name}/{entity.Name}Configuration.cs"
            };
        }

        /// <summary>
        /// 由模块元数据生成实体管理控制器类代码
        /// </summary>
        /// <param name="entity">实体元数据</param>
        /// <returns>实体管理控制器类代码</returns>
        public virtual CodeFile GenerateAdminController(EntityMetadata entity)
        {
            string template = GetInternalTemplate(CodeType.EntityController);
            string code = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), entity.GetType(), entity);
            return new CodeFile()
            {
                SourceCode = code,
                FileName = $"{entity.Module.Project.NamespacePrefix}.Web/Areas/Admin/Controllers/{entity.Module.Name}/{entity.Name}Controller.cs"
            };
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