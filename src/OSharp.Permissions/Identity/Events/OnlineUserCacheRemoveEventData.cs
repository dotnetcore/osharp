// -----------------------------------------------------------------------
//  <copyright file="OnlineUserCacheRemoveEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-09 16:06</last-date>
// -----------------------------------------------------------------------

using OSharp.EventBuses;


namespace OSharp.Identity.Events
{
    /// <summary>
    /// 在线用户信息缓存移除事件数据
    /// </summary>
    public class OnlineUserCacheRemoveEventData : EventDataBase
    {
        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string[] UserNames { get; set; } = new string[0];
    }
}