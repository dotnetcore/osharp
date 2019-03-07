// -----------------------------------------------------------------------
//  <copyright file="IAuditEntityProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-03-08 4:29</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using OSharp.Audits;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义数据审计信息提供者
    /// </summary>
    public interface IAuditEntityProvider
    {
        /// <summary>
        /// 从指定上下文中获取数据审计信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <returns>数据审计信息的集合</returns>
        IEnumerable<AuditEntityEntry> GetAuditEntities(DbContext context);
    }
}