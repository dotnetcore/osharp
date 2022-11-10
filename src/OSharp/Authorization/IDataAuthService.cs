// -----------------------------------------------------------------------
//  <copyright file="IDataAuthService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-17 12:46</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Authorization;

/// <summary>
/// 定义数据权限服务
/// </summary>
public interface IDataAuthService
{
    /// <summary>
    /// 获取指定实体的数据权限过滤表达式
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="operation">数据权限操作</param>
    /// <param name="group">传入的查询条件组，为空时则只返回数据权限过滤器</param>
    /// <returns>实体的数据权限过滤表达式</returns>
    Expression<Func<TEntity, bool>> GetDataFilter<TEntity>(DataAuthOperation operation, FilterGroup group = null);

    /// <summary>
    /// 检查指定操作的数据权限，验证要操作的数据是否符合特定的验证委托
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="operation">数据权限操作</param>
    /// <param name="entities">待检测的实体数据</param>
    /// <returns>是否有权限</returns>
    bool CheckDataAuth<TEntity>(DataAuthOperation operation, params TEntity[] entities);
}