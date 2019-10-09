// -----------------------------------------------------------------------
//  <copyright file="SignalrPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2018-07-26 12:15</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.SignalR;
using OSharp.AspNetCore.SignalR;
using System;
using System.ComponentModel;


namespace Liuliu.Demo.Web.Startups
{
#if NETCOREAPP2_2
    /// <summary>
    /// SignalR模块
    /// </summary>
    [Description("SignalR模块")]
    public class SignalRPack : SignalRPackBase
    {
        /// <summary>
        /// 重写以获取Hub路由创建委托
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns></returns>
        protected override Action<HubRouteBuilder> GetHubRouteBuildAction(IServiceProvider serviceProvider)
        {
            return new Action<HubRouteBuilder>(builder =>
            {
                // 在这实现Hub的路由映射
                // 例如：builder.MapHub<MyHub>();
            });
        } 
    }
#endif
}