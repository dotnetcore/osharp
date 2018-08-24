// -----------------------------------------------------------------------
//  <copyright file="IOnlineUserProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-17 22:30</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;


namespace OSharp.Identity
{
    /// <summary>
    /// 在线用户提供者
    /// </summary>
    public interface IOnlineUserProvider
    {
        /// <summary>
        /// 创建在线用户信息
        /// </summary>
        /// <param name="provider">服务提供器</param>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        Task<OnlineUser> Create(IServiceProvider provider, string userName);
    }
}