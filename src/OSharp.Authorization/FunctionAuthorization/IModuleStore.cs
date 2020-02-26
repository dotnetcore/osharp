// -----------------------------------------------------------------------
//  <copyright file="IModuleStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 12:48</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Authorization.Dtos;
using OSharp.Authorization.Entities;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Entity;


namespace OSharp.Authorization
{
    /// <summary>
    /// 定义模块信息的存储
    /// </summary>
    [IgnoreDependency]
    public interface IModuleStore<TModule, in TModuleInputDto, TModuleKey>
        where TModule : ModuleBase<TModuleKey>, IEntity<TModuleKey>
        where TModuleInputDto : ModuleInputDtoBase<TModuleKey>
        where TModuleKey : struct, IEquatable<TModuleKey>
    {
        #region 模块信息业务

        /// <summary>
        /// 获取 模块信息查询数据集
        /// </summary>
        IQueryable<TModule> Modules { get; }

        /// <summary>
        /// 检查模块信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块信息编号</param>
        /// <returns>模块信息是否存在</returns>
        Task<bool> CheckModuleExists(Expression<Func<TModule, bool>> predicate, TModuleKey id = default(TModuleKey));

        /// <summary>
        /// 添加模块信息
        /// </summary>
        /// <param name="dto">要添加的模块信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> CreateModule(TModuleInputDto dto);

        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="dto">包含更新信息的模块信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateModule(TModuleInputDto dto);

        /// <summary>
        /// 删除模块信息
        /// </summary>
        /// <param name="id">要删除的模块信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteModule(TModuleKey id);

        /// <summary>
        /// 获取树节点及其子节点的所有模块编号
        /// </summary>
        /// <param name="rootIds">树节点</param>
        /// <returns>模块编号集合</returns>
        TModuleKey[] GetModuleTreeIds(params TModuleKey[] rootIds);

        #endregion
    }
}