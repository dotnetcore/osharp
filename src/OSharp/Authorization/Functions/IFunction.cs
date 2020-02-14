// -----------------------------------------------------------------------
//  <copyright file="IFunction.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:14</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;


namespace OSharp.Authorization.Functions
{
    /// <summary>
    /// 定义功能信息
    /// </summary>
    public interface IFunction : IEntity<Guid>, ILockable, IEntityHash
    {
        /// <summary>
        /// 获取或设置 功能名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 获取或设置 区域名称
        /// </summary>
        string Area { get; set; }

        /// <summary>
        /// 获取或设置 控制器名称
        /// </summary>
        string Controller { get; set; }

        /// <summary>
        /// 获取或设置 控制器的功能名称
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// 获取或设置 是否是控制器
        /// </summary>
        bool IsController { get; set; }

        /// <summary>
        /// 获取或设置 是否Ajax功能
        /// </summary>
        bool IsAjax { get; set; }

        /// <summary>
        /// 获取或设置 访问类型
        /// </summary>
        FunctionAccessType AccessType { get; set; }

        /// <summary>
        /// 获取或设置 访问类型是否理发，如为true，刷新功能时将忽略功能类型
        /// </summary>
        bool IsAccessTypeChanged { get; set; }

        /// <summary>
        /// 获取或设置 是否启用操作审计
        /// </summary>
        bool AuditOperationEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否启用数据审计
        /// </summary>
        bool AuditEntityEnabled { get; set; }

        /// <summary>
        /// 获取或设置 数据缓存时间（秒）
        /// </summary>
        int CacheExpirationSeconds { get; set; }

        /// <summary>
        /// 获取或设置 是否相对过期时间，否则为绝对过期
        /// </summary>
        bool IsCacheSliding { get; set; }
    }
}