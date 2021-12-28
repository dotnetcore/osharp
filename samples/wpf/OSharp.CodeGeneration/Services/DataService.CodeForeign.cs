// -----------------------------------------------------------------------
//  <copyright file="DataService.CodeForeign.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-08 23:22</last-date>
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
using OSharp.Extensions;
using OSharp.Mapping;


namespace OSharp.CodeGeneration.Services
{
    public partial class DataService
    {
        /// <summary>
        /// 获取 实体外键信息查询数据集
        /// </summary>
        public IQueryable<CodeForeign> CodeForeigns => ForeignRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查实体外键信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的实体外键信息编号</param>
        /// <returns>实体外键信息是否存在</returns>
        public Task<bool> CheckCodeForeignExists(Expression<Func<CodeForeign, bool>> predicate, Guid id = default)
        {
            return ForeignRepository.CheckExistsAsync(predicate, id);
        }
        
        /// <summary>
        /// 更新实体外键信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的实体外键信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> UpdateCodeForeigns(params CodeForeignInputDto[] dtos)
        {
            List<string> names = new List<string>();
            UnitOfWork.EnableTransaction();
            foreach (var dto in dtos)
            {
                dto.Validate();
                CodeEntity entity = await EntityRepository.GetAsync(dto.EntityId);
                if (entity == null)
                {
                    return new OperationResult(OperationResultType.Error, $"编号为“{dto.EntityId}”的实体信息不存在");
                }
                if (await CheckCodeForeignExists(m => m.SelfNavigation == dto.SelfNavigation && m.EntityId == dto.EntityId, dto.Id))
                {
                    return new OperationResult(OperationResultType.Error, $"实体“{entity.Name}”中名称为“{dto.SelfNavigation}”的外键信息已存在");
                }

                int count;
                if (dto.Id == default)
                {
                    CodeForeign foreign = dto.MapTo<CodeForeign>();
                    count = await ForeignRepository.InsertAsync(foreign);
                }
                else
                {
                    CodeForeign foreign = await ForeignRepository.GetAsync(dto.Id);
                    foreign = dto.MapTo(foreign);
                    count = await ForeignRepository.UpdateAsync(foreign);
                }

                if (count > 0)
                {
                    names.Add($"{entity.Name}-{dto.SelfNavigation}");
                }
            }

            await UnitOfWork.CommitAsync();
            return names.Count > 0
                ? new OperationResult(OperationResultType.Success, $"导航属性“{names.ExpandAndToString()}”更新成功")
                : OperationResult.NoChanged;
        }

        /// <summary>
        /// 删除实体外键信息信息
        /// </summary>
        /// <param name="ids">要删除的实体外键信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteCodeForeigns(params Guid[] ids)
        {
            List<string> names = new List<string>();
            UnitOfWork.EnableTransaction();
            foreach (var id in ids)
            {
                var entity = ForeignRepository.Query(m => m.Id == id).Select(m => new { D = m, EntityName = m.Entity.Name }).FirstOrDefault();
                if (entity == null)
                {
                    continue;
                }

                int count = await ForeignRepository.DeleteAsync(entity.D);
                if (count > 0)
                {
                    names.Add($"{entity.EntityName}-{entity.D.SelfNavigation}");
                }
            }

            await UnitOfWork.CommitAsync();
            return names.Count > 0
                ? new OperationResult(OperationResultType.Success, $"实体外键“{names.ExpandAndToString()}”删除成功")
                : OperationResult.NoChanged;
        }
    }
}
