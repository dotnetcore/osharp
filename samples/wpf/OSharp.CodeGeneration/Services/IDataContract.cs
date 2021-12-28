// -----------------------------------------------------------------------
//  <copyright file="IDataContract.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-06 12:45</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.Data;


namespace OSharp.CodeGeneration.Services
{
    /// <summary>
    /// 数据服务契约
    /// </summary>
    public interface IDataContract
    {
        #region 项目信息业务

        /// <summary>
        /// 获取 项目信息查询数据集
        /// </summary>
        IQueryable<CodeProject> CodeProjects { get; }

        /// <summary>
        /// 检查项目信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的项目信息编号</param>
        /// <returns>项目信息是否存在</returns>
        Task<bool> CheckCodeProjectExists(Expression<Func<CodeProject, bool>> predicate, Guid id = default);

        /// <summary>
        /// 获取指定条件的项目信息
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <returns>项目信息集合</returns>
        CodeProject[] GetCodeProject(Expression<Func<CodeProject, bool>>predicate);
        
        /// <summary>
        /// 添加项目信息信息
        /// </summary>
        /// <param name="dtos">要添加的项目信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> CreateCodeProjects(params CodeProjectInputDto[] dtos);

        /// <summary>
        /// 更新项目信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的项目信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateCodeProjects(params CodeProjectInputDto[] dtos);

        /// <summary>
        /// 删除项目信息信息
        /// </summary>
        /// <param name="ids">要删除的项目信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCodeProjects(params Guid[] ids);

        #endregion
        
        #region 代码模块信息业务

        /// <summary>
        /// 获取 代码模块信息查询数据集
        /// </summary>
        IQueryable<CodeModule> CodeModules { get; }

        /// <summary>
        /// 检查代码模块信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的代码模块信息编号</param>
        /// <returns>代码模块信息是否存在</returns>
        Task<bool> CheckCodeModuleExists(Expression<Func<CodeModule, bool>> predicate, Guid id = default);
        
        /// <summary>
        /// 更新代码模块信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的代码模块信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateCodeModules(params CodeModuleInputDto[] dtos);
        
        /// <summary>
        /// 删除代码模块信息信息
        /// </summary>
        /// <param name="ids">要删除的代码模块信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCodeModules(params Guid[] ids);
        
        #endregion
        
        #region 代码实体信息业务

        /// <summary>
        /// 获取 代码实体信息查询数据集
        /// </summary>
        IQueryable<CodeEntity> CodeEntities { get; }

        /// <summary>
        /// 检查代码实体信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的代码实体信息编号</param>
        /// <returns>代码实体信息是否存在</returns>
        Task<bool> CheckCodeEntityExists(Expression<Func<CodeEntity, bool>> predicate, Guid id = default);

        /// <summary>
        /// 更新代码实体信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的代码实体信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateCodeEntities(params CodeEntityInputDto[] dtos);

        /// <summary>
        /// 删除代码实体信息信息
        /// </summary>
        /// <param name="ids">要删除的代码实体信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCodeEntities(params Guid[] ids);

        #endregion
        
        #region 实体属性信息业务

        /// <summary>
        /// 获取 实体属性信息查询数据集
        /// </summary>
        IQueryable<CodeProperty> CodeProperties { get; }

        /// <summary>
        /// 检查实体属性信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的实体属性信息编号</param>
        /// <returns>实体属性信息是否存在</returns>
        Task<bool> CheckCodePropertyExists(Expression<Func<CodeProperty, bool>> predicate, Guid id = default);
        
        /// <summary>
        /// 更新实体属性信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的实体属性信息输入DTO</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateCodeProperties(params CodePropertyInputDto[] dtos);

        /// <summary>
        /// 删除实体属性信息信息
        /// </summary>
        /// <param name="ids">要删除的实体属性信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCodeProperties(params Guid[] ids);

        #endregion

        #region 实体外键信息业务

        /// <summary>
        /// 获取 实体外键信息查询数据集
        /// </summary>
        IQueryable<CodeForeign> CodeForeigns { get; }

        /// <summary>
        /// 检查实体外键信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的实体外键信息编号</param>
        /// <returns>实体外键信息是否存在</returns>
        Task<bool> CheckCodeForeignExists(Expression<Func<CodeForeign, bool>> predicate, Guid id = default);

        /// <summary>
        /// 更新实体外键信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的实体外键信息输入DTO</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateCodeForeigns(params CodeForeignInputDto[] dtos);
        
        /// <summary>
        /// 删除实体外键信息信息
        /// </summary>
        /// <param name="ids">要删除的实体外键信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCodeForeigns(params Guid[] ids);

        #endregion
        
        #region 代码模板信息业务

        /// <summary>
        /// 获取 代码模板信息查询数据集
        /// </summary>
        IQueryable<CodeTemplate> CodeTemplates { get; }

        /// <summary>
        /// 检查代码模板信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的代码模板信息编号</param>
        /// <returns>代码模板信息是否存在</returns>
        Task<bool> CheckCodeTemplateExists(Expression<Func<CodeTemplate, bool>> predicate, Guid id = default);
        
        /// <summary>
        /// 更新代码模板信息信息
        /// </summary>
        /// <param name="settings">包含更新信息的代码模板信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult<CodeTemplate[]>> UpdateCodeTemplates(params CodeTemplateInputDto[] settings);

        /// <summary>
        /// 删除代码模板信息信息
        /// </summary>
        /// <param name="ids">要删除的代码模板信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCodeTemplates(params Guid[] ids);

        #endregion

        #region 项目模板信息业务

        /// <summary>
        /// 获取 项目模板信息查询数据集
        /// </summary>
        IQueryable<CodeProjectTemplate> CodeProjectTemplates { get; }

        /// <summary>
        /// 检查项目模板信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的项目模板信息编号</param>
        /// <returns>项目模板信息是否存在</returns>
        Task<bool> CheckCodeProjectTemplateExists(Expression<Func<CodeProjectTemplate, bool>> predicate, Guid id = default);
        
        /// <summary>
        /// 更新项目模板信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的项目模板信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateCodeProjectTemplates(params CodeProjectTemplateInputDto[] dtos);

        /// <summary>
        /// 删除项目模板信息信息
        /// </summary>
        /// <param name="ids">要删除的项目模板信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCodeProjectTemplates(params Guid[] ids);

        #endregion

    }
}
