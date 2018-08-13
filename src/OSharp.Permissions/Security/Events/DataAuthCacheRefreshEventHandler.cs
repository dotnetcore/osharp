// -----------------------------------------------------------------------
//  <copyright file="DataAuthCacheRefreshEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-05 4:28</last-date>
// -----------------------------------------------------------------------

using OSharp.Dependency;
using OSharp.EventBuses;
using OSharp.Secutiry;


namespace OSharp.Security.Events
{
    /// <summary>
    /// 数据权限缓存刷新事件处理器
    /// </summary>
    public class DataAuthCacheRefreshEventHandler : EventHandlerBase<DataAuthCacheRefreshEventData>, ITransientDependency
    {
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(DataAuthCacheRefreshEventData eventData)
        {
            IDataAuthCache cache = ServiceLocator.Instance.GetService<IDataAuthCache>();
            //更新缓存项
            foreach (DataAuthCacheItem cacheItem in eventData.SetItems)
            {
                cache.SetCache(cacheItem);
            }
            //移除缓存项
            foreach (DataAuthCacheItem cacheItem in eventData.RemoveItems)
            {
                cache.RemoveCache(cacheItem);
            }
        }
    }
}