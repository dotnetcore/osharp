// -----------------------------------------------------------------------
//  <copyright file="ISecurityContract.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-10-24 23:53</last-date>
// -----------------------------------------------------------------------

using System.Linq;

using OSharp.Dependency;
using OSharp.Infrastructure;


namespace OSharp.Demo.Security
{
    /// <summary>
    /// 业务契约：权限安全模块
    /// </summary>
    public interface ISecurityContract : IScopeDependency
    {
        /// <summary>
        /// 获取 功能数据集
        /// </summary>
        IQueryable<Function> Functions { get; }
    }
}