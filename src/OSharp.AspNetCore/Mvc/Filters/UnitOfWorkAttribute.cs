// -----------------------------------------------------------------------
//  <copyright file="UnitOfWorkAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-12-03 14:12</last-date>
// -----------------------------------------------------------------------


namespace OSharp.AspNetCore.Mvc.Filters;

/// <summary>
/// 自动提交工作单元事务
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UnitOfWorkAttribute : ServiceFilterAttribute
{
    /// <summary>
    /// 自动提交工作单元事务
    /// </summary>
    public UnitOfWorkAttribute()
        : base(typeof(UnitOfWorkImpl))
    { }
}
