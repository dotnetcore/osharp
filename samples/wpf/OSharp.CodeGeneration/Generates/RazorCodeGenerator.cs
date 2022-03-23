// -----------------------------------------------------------------------
//  <copyright file="RazorCodeGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 21:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniRazor;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Exceptions;


namespace OSharp.CodeGeneration.Generates
{
    /// <summary>
    /// Razor代码生成器
    /// </summary>
    public class RazorCodeGenerator : ICodeGenerator
    {
        /// <summary>
        /// 生成项目所有代码
        /// </summary>
        /// <param name="templates">要处理的代码配置集合</param>
        /// <param name="project">代码项目信息</param>
        /// <returns>输出的代码文件信息集合</returns>
        public async Task<CodeFile[]> GenerateCodes(CodeTemplate[] templates, CodeProject project)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            CodeModule[] modules = project.Modules.ToArray();
            CodeEntity[] entities = modules.SelectMany(m => m.Entities).ToArray();

            CodeTemplate[] templates2 = templates.Where(m => m.MetadataType == MetadataType.Entity).ToArray();
            foreach (CodeTemplate template in templates2)
            {
                foreach (CodeEntity entity in entities)
                {
                    codeFiles.Add(await GenerateCode(template, entity));
                }
            }

            templates2 = templates.Where(m => m.MetadataType == MetadataType.Module).ToArray();
            foreach (CodeTemplate template in templates2)
            {
                foreach (CodeModule module in modules)
                {
                    codeFiles.Add(await GenerateCode(template, module));
                }
            }

            templates2 = templates.Where(m => m.MetadataType == MetadataType.Project).ToArray();
            foreach (CodeTemplate template in templates2)
            {
                codeFiles.Add(await GenerateCode(template, project));
            }

            return codeFiles.OrderBy(m => m.FileName).ToArray();
        }

        /// <summary>
        /// 生成项目元数据相关代码
        /// <param name="template">代码配置</param>
        /// <param name="project">项目元数据</param>
        /// </summary>
        public Task<CodeFile> GenerateCode(CodeTemplate template, CodeProject project)
        {
            string fileName = template.GetCodeFileName(project);
            return GenerateCodeCore(template, project, fileName);
        }

        /// <summary>
        /// 生成模块元数据相关代码
        /// <param name="template">代码配置</param>
        /// <param name="module">模块元数据</param>
        /// </summary>
        public Task<CodeFile> GenerateCode(CodeTemplate template, CodeModule module)
        {
            string fileName = template.GetCodeFileName(module);
            return GenerateCodeCore(template, module, fileName);
        }

        /// <summary>
        /// 生成实体元数据相关代码
        /// <param name="template">代码配置</param>
        /// <param name="entity">实体元数据</param>
        /// </summary>
        public Task<CodeFile> GenerateCode(CodeTemplate template, CodeEntity entity)
        {
            string fileName = template.GetCodeFileName(entity);
            return GenerateCodeCore(template, entity, fileName);
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="template">代码配置</param>
        /// <param name="model">代码数据模型</param>
        /// <param name="fileName">代码输出文件</param>
        /// <returns></returns>
        protected virtual async Task<CodeFile> GenerateCodeCore(CodeTemplate template, object model, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            await using TextWriter writer = new StringWriter(sb);
            if (template.TemplateFile == "内置")
            {
                Type innerTemplateType = template.GetInnerTemplateType();
                ITemplate template2 = (ITemplate)(Activator.CreateInstance(innerTemplateType)
                    ?? throw new OsharpException($"代码配置“{template.Name}”的模板类型实例化失败"));
                template2.Model = model;
                template2.Output = writer;
                await template2.ExecuteAsync();
            }
            else
            {
                if (template.TemplateFile == null || !File.Exists(template.TemplateFile))
                {
                    throw new OsharpException($"代码配置“{template.Name}”的模板文件“{template.TemplateFile}”不存在");
                }

                string templateSource = await File.ReadAllTextAsync(template.TemplateFile);
                TemplateDescriptor descriptor = Razor.Compile(templateSource);
                await descriptor.RenderAsync(writer, model);
            }

            string codeSource = sb.ToString();

            CodeFile codeFile = new CodeFile()
            {
                Template = template,
                SourceCode = codeSource,
                FileName = fileName
            };
            return codeFile;
        }
    }
}
