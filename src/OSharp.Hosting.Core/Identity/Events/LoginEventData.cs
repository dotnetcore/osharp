// -----------------------------------------------------------------------
//  <copyright file="LoginEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;

using OSharp.EventBuses;


namespace OSharp.Hosting.Identity.Events
{
    /// <summary>
    /// 登录事件数据
    /// </summary>
    public class LoginEventData : EventDataBase
    {
        /// <summary>
        /// 获取或设置 登录信息
        /// </summary>
        public LoginDto LoginDto { get; set; }

        /// <summary>
        /// 获取或设置 登录用户
        /// </summary>
        public User User { get; set; }
    }
}