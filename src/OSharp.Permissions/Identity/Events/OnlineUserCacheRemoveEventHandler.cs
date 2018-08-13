// -----------------------------------------------------------------------
//  <copyright file="OnlineUserCacheRemoveEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-09 16:07</last-date>
// -----------------------------------------------------------------------

using OSharp.Dependency;
using OSharp.EventBuses;


namespace OSharp.Identity.Events
{
    /// <summary>
    /// 在线用户信息缓存移除事件处理器
    /// </summary>
    public class OnlineUserCacheRemoveEventHandler : EventHandlerBase<OnlineUserCacheRemoveEventData>, ITransientDependency
    {
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(OnlineUserCacheRemoveEventData eventData)
        {
            IOnlineUserCache onlineUserCache = ServiceLocator.Instance.GetService<IOnlineUserCache>();
            onlineUserCache.Remove(eventData.UserNames);
        }
    }
}