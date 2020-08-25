// -----------------------------------------------------------------------
//  <copyright file="FunctionBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:13</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;

using OSharp.Entity;


namespace OSharp.Authorization.Functions
{
    /// <summary>
    /// 功能信息基类
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public abstract class FunctionBase : EntityBase<Guid>, IFunction
    {
        /// <summary>
        /// 获取或设置 功能名称
        /// </summary>
        [DisplayName("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 区域名称
        /// </summary>
        [DisplayName("区域")]
        public string Area { get; set; }

        /// <summary>
        /// 获取或设置 控制器名称
        /// </summary>
        [DisplayName("控制器")]
        public string Controller { get; set; }

        /// <summary>
        /// 获取或设置 控制器的功能名称
        /// </summary>
        [DisplayName("功能")]
        public string Action { get; set; }

        /// <summary>
        /// 获取或设置 是否是控制器
        /// </summary>
        [DisplayName("是否控制器")]
        public bool IsController { get; set; }

        /// <summary>
        /// 获取或设置 是否Ajax功能
        /// </summary>
        [DisplayName("是否Ajax功能")]
        public bool IsAjax { get; set; }

        /// <summary>
        /// 获取或设置 访问类型
        /// </summary>
        [DisplayName("访问类型")]
        public FunctionAccessType AccessType { get; set; }

        /// <summary>
        /// 获取或设置 访问类型是否更改，如为true，刷新功能时将忽略功能类型
        /// </summary>
        [DisplayName("访问类型是否更改")]
        public bool IsAccessTypeChanged { get; set; }

        /// <summary>
        /// 获取或设置 是否启用操作审计
        /// </summary>
        [DisplayName("是否操作审计")]
        public bool AuditOperationEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否启用数据审计
        /// </summary>
        [DisplayName("是否数据审计")]
        public bool AuditEntityEnabled { get; set; }

        /// <summary>
        /// 获取或设置 数据缓存时间（秒）
        /// </summary>
        [DisplayName("数据缓存秒数")]
        public int CacheExpirationSeconds { get; set; }

        /// <summary>
        /// 获取或设置 是否相对过期时间，否则为绝对过期
        /// </summary>
        [DisplayName("是否相对过期时间")]
        public bool IsCacheSliding { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        [DisplayName("是否锁定")]
        public bool IsLocked { get; set; }

        /// <summary>返回一个表示当前对象的 string。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return $"{Name}[{Area}/{Controller}/{Action}]";
        }
    }
}