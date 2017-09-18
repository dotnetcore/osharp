// -----------------------------------------------------------------------
//  <copyright file="FunctionBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-14 20:06</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Entity;


namespace OSharp.Infrastructure
{
    /// <summary>
    /// 功能信息基类
    /// </summary>
    public abstract class FunctionBase : EntityBase<Guid>, IFunction
    {
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
        /// 获取或设置 访问类型是否理发，如为true，刷新功能时将忽略功能类型
        /// </summary>
        public bool IsAccessTypeChanged { get; set; }

        /// <summary>
        /// 获取或设置 是否启用操作日志
        /// </summary>
        public bool OperateLogEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否启用数据日志
        /// </summary>
        public bool DataLogEnabled { get; set; }

        /// <summary>
        /// 获取或设置 数据缓存时间（秒）
        /// </summary>
        public int CacheExpirationSeconds { get; set; }

        /// <summary>
        /// 获取或设置 是否相对过期时间，否则为绝对过期
        /// </summary>
        public bool IsCacheSliding { get; set; }

        /// <summary>
        /// 获取或设置 是否锁定当前信息
        /// </summary>
        public bool IsLocked { get; set; }
    }
}