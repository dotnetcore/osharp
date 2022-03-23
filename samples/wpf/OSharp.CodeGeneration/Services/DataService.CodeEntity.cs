// -----------------------------------------------------------------------
//  <copyright file="DataService.Entity.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-07 1:20</last-date>
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
        /// 获取 代码实体信息查询数据集
        /// </summary>
        public IQueryable<CodeEntity> CodeEntities => EntityRepository.Query();

        /// <summary>
        /// 检查代码实体信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的代码实体信息编号</param>
        /// <returns>代码实体信息是否存在</returns>
        public Task<bool> CheckCodeEntityExists(Expression<Func<CodeEntity, bool>> predicate, Guid id = default)
        {
            return EntityRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 更新代码实体信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的代码实体信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> UpdateCodeEntities(params CodeEntityInputDto[] dtos)
        {
            List<string> names = new List<string>();
            UnitOfWork.EnableTransaction();
            foreach (var dto in dtos)
            {
                dto.Validate();
                CodeModule module = await ModuleRepository.GetAsync(dto.ModuleId);
                if (module == null)
                {
                    return new OperationResult(OperationResultType.Error, $"编号为“{dto.ModuleId}”的模块信息不存在");
                }

                if (await CheckCodeEntityExists(m => m.Name == dto.Name && m.ModuleId == dto.ModuleId, dto.Id))
                {
                    return new OperationResult(OperationResultType.Error, $"模块“{module.Name}”中名称为“{dto.Name}”的实体信息已存在");
                }

                if (dto.Order == 0)
                {
                    dto.Order = EntityRepository.Query(m => m.ModuleId == module.Id).Count() + 1;
                }
                int count;
                if (dto.Id == default)
                {
                    CodeEntity entity = dto.MapTo<CodeEntity>();
                    count = await EntityRepository.InsertAsync(entity);
                }
                else
                {
                    CodeEntity entity = await EntityRepository.GetAsync(dto.Id);
                    entity = dto.MapTo(entity);
                    count = await EntityRepository.UpdateAsync(entity);
                }

                if (count > 0)
                {
                    names.Add(dto.Name);
                }
            }

            await UnitOfWork.CommitAsync();
            return names.Count > 0
                ? new OperationResult(OperationResultType.Success, $"实体“{names.ExpandAndToString()}”保存成功")
                : OperationResult.NoChanged;
        }
        
        /// <summary>
        /// 删除代码实体信息信息
        /// </summary>
        /// <param name="ids">要删除的代码实体信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteCodeEntities(params Guid[] ids)
        {
            List<string> names = new List<string>();
            UnitOfWork.EnableTransaction();
            foreach (var id in ids)
            {
                var entity = EntityRepository.Query(m => m.Id == id).Select(m => new { D = m, PropertyCount = m.Properties.Count() })
                    .FirstOrDefault();
                if (entity == null)
                {
                    return null;
                }

                if (entity.PropertyCount > 0)
                {
                    return new OperationResult(OperationResultType.Error, $"实体“{entity.D.Name}”包含着 {entity.PropertyCount} 个属性，请先删除下属属性信息");
                }

                int count = await EntityRepository.DeleteAsync(entity.D);
                if (count > 0)
                {
                    names.Add(entity.D.Name);
                }
            }

            await UnitOfWork.CommitAsync();
            return names.Count > 0
                ? new OperationResult(OperationResultType.Success, $"实体“{names.ExpandAndToString()}”删除成功")
                : OperationResult.NoChanged;
        }
    }
}
