// -----------------------------------------------------------------------
//  <copyright file="IModuleRoleStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 15:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;


namespace OSharp.Authorization
{
    /// <summary>
    /// 定义模块角色信息存储
    /// </summary>
    public interface IModuleRoleStore<TModuleRole, in TRoleKey, TModuleKey>
    {
        #region 模块角色信息业务

        /// <summary>
        /// 获取 模块角色信息查询数据集
        /// </summary>
        IQueryable<TModuleRole> ModuleRoles { get; }

        /// <summary>
        /// 检查模块角色信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块角色信息编号</param>
        /// <returns>模块角色信息是否存在</returns>
        Task<bool> CheckModuleRoleExists(Expression<Func<TModuleRole, bool>> predicate, Guid id = default(Guid));

        /// <summary>
        /// 设置角色的可访问模块
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="moduleIds">要赋予的模块编号集合</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SetRoleModules(TRoleKey roleId, TModuleKey[] moduleIds);

        /// <summary>
        /// 获取角色可访问模块编号
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>模块编号集合</returns>
        TModuleKey[] GetRoleModuleIds(TRoleKey roleId);

        #endregion
    }
}