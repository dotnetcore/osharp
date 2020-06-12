// -----------------------------------------------------------------------
//  <copyright file="FunctionOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Authorization.Functions;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Hosting.Authorization.Dtos
{
    /// <summary>
    /// 功能输出DTO
    /// </summary>
    [MapFrom(typeof(Function))]
    public class FunctionOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 区域名称
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 获取或设置 控制器名称
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 获取或设置 控制器的功能名称
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 获取或设置 是否是控制器
        /// </summary>
        public bool IsController { get; set; }

        /// <summary>
        /// 获取或设置 是否Ajax功能
        /// </summary>
        public bool IsAjax { get; set; }

        /// <summary>
        /// 获取或设置 访问类型
        /// </summary>
        public FunctionAccessType AccessType { get; set; }

        /// <summary>
        /// 获取或设置 访问类型是否更改，如为true，刷新功能时将忽略功能类型
        /// </summary>
        public bool IsAccessTypeChanged { get; set; }

        /// <summary>
        /// 获取或设置 是否启用操作审计
        /// </summary>
        public bool AuditOperationEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否启用数据审计
        /// </summary>
        public bool AuditEntityEnabled { get; set; }

        /// <summary>
        /// 获取或设置 数据缓存时间（秒）
        /// </summary>
        public int CacheExpirationSeconds { get; set; }

        /// <summary>
        /// 获取或设置 是否相对过期时间，否则为绝对过期
        /// </summary>
        public bool IsCacheSliding { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        #region Implementation of IDataAuthEnabled

        /// <summary>
        /// 获取或设置 是否可更新的数据权限状态
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// 获取或设置 是否可删除的数据权限状态
        /// </summary>
        public bool Deletable { get; set; }

        #endregion
    }
}