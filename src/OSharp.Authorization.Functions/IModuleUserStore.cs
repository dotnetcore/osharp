// -----------------------------------------------------------------------
//  <copyright file="IModuleUserStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 15:51</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Dependency;


namespace OSharp.Authorization
{
    /// <summary>
    /// 模块用户信息存储
    /// </summary>
    [IgnoreDependency]
    public interface IModuleUserStore<TModuleUser, in TUserKey, TModuleKey>
    {
        #region 模块用户信息业务

        /// <summary>
        /// 获取 模块用户信息查询数据集
        /// </summary>
        IQueryable<TModuleUser> ModuleUsers { get; }

        /// <summary>
        /// 检查模块用户信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块用户信息编号</param>
        /// <returns>模块用户信息是否存在</returns>
        Task<bool> CheckModuleUserExists(Expression<Func<TModuleUser, bool>> predicate, Guid id = default(Guid));

        /// <summary>
        /// 设置用户的可访问模块
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="moduleIds">要赋给用户的模块编号集合</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SetUserModules(TUserKey userId, TModuleKey[] moduleIds);

        /// <summary>
        /// 获取用户自己的可访问模块编号
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>模块编号集合</returns>
        TModuleKey[] GetUserSelfModuleIds(TUserKey userId);

        /// <summary>
        /// 获取用户及其拥有角色可访问模块编号
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>模块编号集合</returns>
        TModuleKey[] GetUserWithRoleModuleIds(TUserKey userId);

        #endregion
    }
}