// -----------------------------------------------------------------------
//  <copyright file="IModuleFunctionStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 15:44</last-date>
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
    /// 定义模块功能存储
    /// </summary>
    [IgnoreDependency]
    public interface IModuleFunctionStore<TModuleFunction, in TModuleKey>
    {
        #region 模块功能信息业务

        /// <summary>
        /// 获取 模块功能信息查询数据集
        /// </summary>
        IQueryable<TModuleFunction> ModuleFunctions { get; }

        /// <summary>
        /// 检查模块功能信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的模块功能信息编号</param>
        /// <returns>模块功能信息是否存在</returns>
        Task<bool> CheckModuleFunctionExists(Expression<Func<TModuleFunction, bool>> predicate, Guid id = default(Guid));

        /// <summary>
        /// 设置模块的功能信息
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <param name="functionIds">要设置的功能编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> SetModuleFunctions(TModuleKey moduleId, Guid[] functionIds);

        #endregion
    }
}