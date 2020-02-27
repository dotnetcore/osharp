// -----------------------------------------------------------------------
//  <copyright file="IEntityRoleStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-03 22:52</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Filter;


namespace OSharp.Authorization
{
    /// <summary>
    /// 定义实体角色信息存储
    /// </summary>
    public interface IEntityRoleStore<TEntityRole, in TEntityRoleInputDto, in TRoleKey>
    {
        #region 实体角色信息业务

        /// <summary>
        /// 获取 实体角色信息查询数据集
        /// </summary>
        IQueryable<TEntityRole> EntityRoles { get; }

        /// <summary>
        /// 检查实体角色信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的实体角色信息编号</param>
        /// <returns>实体角色信息是否存在</returns>
        Task<bool> CheckEntityRoleExists(Expression<Func<TEntityRole, bool>> predicate, Guid id = default(Guid));

        /// <summary>
        /// 获取指定角色和实体的过滤条件组
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="entityId">实体编号</param>
        /// <param name="operation">操作</param>
        /// <returns>过滤条件组</returns>
        FilterGroup[] GetEntityRoleFilterGroups(TRoleKey roleId, Guid entityId, DataAuthOperation operation);

        /// <summary>
        /// 添加实体角色信息
        /// </summary>
        /// <param name="dtos">要添加的实体角色信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> CreateEntityRoles(params TEntityRoleInputDto[] dtos);

        /// <summary>
        /// 更新实体角色信息
        /// </summary>
        /// <param name="dtos">包含更新信息的实体角色信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEntityRoles(params TEntityRoleInputDto[] dtos);

        /// <summary>
        /// 删除实体角色信息
        /// </summary>
        /// <param name="ids">要删除的实体角色信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteEntityRoles(params Guid[] ids);

        #endregion

    }
}