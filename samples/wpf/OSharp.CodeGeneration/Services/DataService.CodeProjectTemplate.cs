// -----------------------------------------------------------------------
//  <copyright file="DataService.CodeProjectTemplate.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-12 16:13</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Exceptions;
using OSharp.Extensions;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services
{
    public partial class DataService
    {
        /// <summary>
        /// 获取 项目模板信息查询数据集
        /// </summary>
        public IQueryable<CodeProjectTemplate> CodeProjectTemplates => ProjectTemplateRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查项目模板信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的项目模板信息编号</param>
        /// <returns>项目模板信息是否存在</returns>
        public Task<bool> CheckCodeProjectTemplateExists(Expression<Func<CodeProjectTemplate, bool>> predicate, Guid id = default)
        {
            return ProjectTemplateRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 更新项目模板信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的项目模板信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> UpdateCodeProjectTemplates(params CodeProjectTemplateInputDto[] dtos)
        {
            string pName = null;
            List<string> names = new List<string>();
            UnitOfWork.EnableTransaction();
            foreach (CodeProjectTemplateInputDto dto in dtos)
            {
                dto.Validate();
                CodeProject project = await ProjectRepository.GetAsync(dto.ProjectId);
                if (project == null)
                {
                    throw new OsharpException($"编号为“{dto.ProjectId}”的项目信息不存在");
                }

                pName ??= project.Name;
                CodeTemplate template = await TemplateRepository.GetAsync(dto.TemplateId);
                if (template == null)
                {
                    throw new OsharpException($"编号为“{dto.TemplateId}”的模板信息不存在");
                }

                if (await CheckCodeProjectTemplateExists(m => m.ProjectId == dto.ProjectId && m.TemplateId == dto.TemplateId, dto.Id))
                {
                    throw new OsharpException($"项目“{project.Name}”中模板“{template.Name}”已存在，不能重复添加");
                }

                int count = 0;
                if (dto.Id ==default)
                {
                    CodeProjectTemplate projectTemplate = dto.MapTo<CodeProjectTemplate>();
                    count = await ProjectTemplateRepository.InsertAsync(projectTemplate);
                }
                else
                {
                    CodeProjectTemplate projectTemplate = await ProjectTemplateRepository.GetAsync(dto.Id);
                    projectTemplate = dto.MapTo(projectTemplate);
                    count = await ProjectTemplateRepository.UpdateAsync(projectTemplate);
                }

                if (count > 0)
                {
                    names.Add(template.Name);
                }
            }

            await UnitOfWork.CommitAsync();
            return names.Count > 0
                ? new OperationResult(OperationResultType.Success, $"项目“{pName}”设置模板“{names.ExpandAndToString()}”成功")
                : OperationResult.NoChanged;
        }

        /// <summary>
        /// 删除项目模板信息信息
        /// </summary>
        /// <param name="ids">要删除的项目模板信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteCodeProjectTemplates(params Guid[] ids)
        {
            UnitOfWork.EnableTransaction();
            OperationResult result = await ProjectTemplateRepository.DeleteAsync(ids);
            await UnitOfWork.CommitAsync();
            return result;
        }
    }
}
