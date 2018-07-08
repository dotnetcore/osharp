// -----------------------------------------------------------------------
//  <copyright file="DataAuthCacheRefreshEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-05 4:22</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using OSharp.EventBuses;
using OSharp.Secutiry;


namespace OSharp.Security.Events
{
    /// <summary>
    /// 数据权限缓存刷新事件数据
    /// </summary>
    public class DataAuthCacheRefreshEventData : EventDataBase
    {
        /// <summary>
        /// 获取或设置 数据权限缓存项集合
        /// </summary>
        public List<DataAuthCacheItem> CacheItems { get; set; }
    }
}