// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthCacheRefreshEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-12 22:29</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.EventBuses;


namespace OSharp.Security.Events
{
    /// <summary>
    /// 功能权限缓存刷新事件源
    /// </summary>
    public class FunctionAuthCacheRefreshEventData : EventDataBase
    {
        /// <summary>
        /// 初始化一个<see cref="FunctionAuthCacheRefreshEventData"/>类型的新实例
        /// </summary>
        public FunctionAuthCacheRefreshEventData()
        {
            FunctionIds = new Guid[0];
            UserNames = new string[0];
        }

        /// <summary>
        /// 获取或设置 功能编号
        /// </summary>
        public Guid[] FunctionIds { get; set; }

        /// <summary>
        /// 获取或设置 用户名集合
        /// </summary>
        public string[] UserNames { get; set; }
    }
}