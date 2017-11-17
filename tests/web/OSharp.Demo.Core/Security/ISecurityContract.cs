// -----------------------------------------------------------------------
//  <copyright file="ISecurityContract.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-10-24 23:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Dependency;
using OSharp.Infrastructure;
using OSharp.Security;


namespace OSharp.Demo.Security
{
    /// <summary>
    /// 业务契约：权限安全模块
    /// </summary>
    public interface ISecurityContract : IScopeDependency
    {
        #region 功能信息业务

        /// <summary>
        /// 获取 功能信息查询数据集
        /// </summary>
        IQueryable<Function> Functions { get; }

        /// <summary>
        /// 检查功能信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的功能信息编号</param>
        /// <returns>功能信息是否存在</returns>
        Task<bool> CheckFunctionExists(Expression<Func<Function, bool>> predicate, Guid id = default(Guid));
        
        /// <summary>
        /// 更新功能信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的功能信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateFunctions(params FunctionInputDto[] dtos);
        
        #endregion

    }
}