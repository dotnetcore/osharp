// -----------------------------------------------------------------------
//  <copyright file="DataAuthCacheRefreshEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-05 4:28</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using OSharp.EventBuses;


namespace OSharp.Authorization.Events
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


    /// <summary>
    /// 数据权限缓存刷新事件处理器
    /// </summary>
    public class DataAuthCacheRefreshEventHandler : EventHandlerBase<DataAuthCacheRefreshEventData>
    {
        private readonly IDataAuthCache _authCache;

        /// <summary>
        /// 初始化一个<see cref="DataAuthCacheRefreshEventHandler"/>类型的新实例
        /// </summary>
        public DataAuthCacheRefreshEventHandler(IDataAuthCache authCache)
        {
            _authCache = authCache;
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(DataAuthCacheRefreshEventData eventData)
        {
            //更新缓存项
            foreach (DataAuthCacheItem cacheItem in eventData.SetItems)
            {
                _authCache.SetCache(cacheItem);
            }
            //移除缓存项
            foreach (DataAuthCacheItem cacheItem in eventData.RemoveItems)
            {
                _authCache.RemoveCache(cacheItem);
            }
        }
    }
}