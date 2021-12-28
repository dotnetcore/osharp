// -----------------------------------------------------------------------
//  <copyright file="DataService.Setting.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 22:45</last-date>
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
        /// 获取 代码设置信息查询数据集
        /// </summary>
        public IQueryable<CodeTemplate> CodeTemplates => TemplateRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查代码设置信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的代码设置信息编号</param>
        /// <returns>代码设置信息是否存在</returns>
        public Task<bool> CheckCodeTemplateExists(Expression<Func<CodeTemplate, bool>> predicate, Guid id = default)
        {
            return TemplateRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 更新代码设置信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的代码设置信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult<CodeTemplate[]>> UpdateCodeTemplates(params CodeTemplateInputDto[] dtos)
        {
            List<string> names = new List<string>();
            List<CodeTemplate> data = new List<CodeTemplate>();
            UnitOfWork.EnableTransaction();
            foreach (var dto in dtos)
            {
                dto.Validate();
                if (await CheckCodeTemplateExists(m => m.Name == dto.Name, dto.Id))
                {
                    return new OperationResult<CodeTemplate[]>(OperationResultType.Error, $"名称为“{dto.Name}”的代码设置已存在");
                }

                int count;
                if (dto.Id == default)
                {
                    CodeTemplate template = dto.MapTo<CodeTemplate>();
                    count = await TemplateRepository.InsertAsync(template);
                    data.Add(template);
                }
                else
                {
                    CodeTemplate template = await TemplateRepository.GetAsync(dto.Id);
                    template = dto.MapTo(template);
                    count = await TemplateRepository.UpdateAsync(template);
                    data.Add(template);
                }

                if (count > 0)
                {
                    names.Add(dto.Name);
                }
            }

            await UnitOfWork.CommitAsync();
            return names.Count > 0
                ? new OperationResult<CodeTemplate[]>(OperationResultType.Success, $"代码设置“{names.ExpandAndToString()}”更新成功", data.ToArray())
                : OperationResult<CodeTemplate[]>.NoChanged;
        }

        /// <summary>
        /// 删除代码设置信息信息
        /// </summary>
        /// <param name="ids">要删除的代码设置信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteCodeTemplates(params Guid[] ids)
        {
            List<string> names = new List<string>();
            UnitOfWork.EnableTransaction();
            foreach (var id in ids)
            {
                var entity = await TemplateRepository.GetAsync(id);
                if (entity == null)
                {
                    continue;
                }

                if (entity.IsSystem)
                {
                    throw new OsharpException($"代码设置“{entity.Name}”是系统设置，不能删除");
                }

                int count = await TemplateRepository.DeleteAsync(entity);
                if (count > 0)
                {
                    names.Add(entity.Name);
                }
            }

            await UnitOfWork.CommitAsync();
            return names.Count > 0
                ? new OperationResult(OperationResultType.Success, $"代码设置“{names.ExpandAndToString()}”删除成功")
                : OperationResult.NoChanged;
        }
    }
}
