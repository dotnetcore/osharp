// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthCacheRefreshEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-12 22:29</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.EventBuses;


namespace OSharp.Authorization.Events
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


    /// <summary>
    /// 功能权限缓存刷新事件处理器
    /// </summary>
    public class FunctionAuthCacheRefreshEventHandler : EventHandlerBase<FunctionAuthCacheRefreshEventData>
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个<see cref="FunctionAuthCacheRefreshEventHandler"/>类型的新实例
        /// </summary>
        public FunctionAuthCacheRefreshEventHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(FunctionAuthCacheRefreshEventData eventData)
        {
            if (!_provider.InHttpRequest())
            {
                return;
            }

            IFunctionAuthCache cache = _provider.GetService<IFunctionAuthCache>();
            if (eventData.FunctionIds.Length > 0)
            {
                cache.RemoveFunctionCaches(eventData.FunctionIds);
                foreach (Guid functionId in eventData.FunctionIds)
                {
                    cache.GetFunctionRoles(functionId, _provider);
                }
            }
            if (eventData.UserNames.Length > 0)
            {
                cache.RemoveUserCaches(eventData.UserNames);
                foreach (string userName in eventData.UserNames)
                {
                    cache.GetUserFunctions(userName);
                }
            }
        }
    }
}