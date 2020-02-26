// -----------------------------------------------------------------------
//  <copyright file="IFunctionStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-15 18:59</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Authorization.Dtos;
using OSharp.Authorization.Functions;
using OSharp.Data;
using OSharp.Dependency;


namespace OSharp.Authorization
{
    /// <summary>
    /// 定义功能信息存储
    /// </summary>
    /// <typeparam name="TFunction">功能信息类型</typeparam>
    /// <typeparam name="TFunctionInputDto">功能信息输入DTO类型</typeparam>
    [IgnoreDependency]
    public interface IFunctionStore<TFunction, in TFunctionInputDto>
        where TFunction : IFunction
        where TFunctionInputDto : FunctionInputDtoBase
    {
        /// <summary>
        /// 获取 功能信息查询数据集
        /// </summary>
        IQueryable<TFunction> Functions { get; }

        /// <summary>
        /// 检查功能信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的功能信息编号</param>
        /// <returns>功能信息是否存在</returns>
        Task<bool> CheckFunctionExists(Expression<Func<TFunction, bool>> predicate, Guid id = default(Guid));

        /// <summary>
        /// 更新功能信息
        /// </summary>
        /// <param name="dtos">包含更新信息的功能信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateFunctions(params TFunctionInputDto[] dtos);

    }
}