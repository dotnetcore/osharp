// -----------------------------------------------------------------------
//  <copyright file="DataAuthCacheRefreshEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-12 1:47</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using OSharp.EventBuses;
using OSharp.Security;


namespace OSharp.Security.Events
{
    /// <summary>
    /// 数据权限缓存刷新事件数据
    /// </summary>
    public class DataAuthCacheRefreshEventData : EventDataBase
    {
        /// <summary>
        /// 获取或设置 要更新的数据权限缓存项集合
        /// </summary>
        public IList<DataAuthCacheItem> SetItems { get; } = new List<DataAuthCacheItem>();

        /// <summary>
        /// 获取或设置 要移除的数据权限缓存项信息
        /// </summary>
        public IList<DataAuthCacheItem> RemoveItems { get; } = new List<DataAuthCacheItem>();

        /// <summary>
        /// 是否有值
        /// </summary>
        public bool HasData()
        {
            return SetItems.Count > 0 || RemoveItems.Count > 0;
        }
    }
}