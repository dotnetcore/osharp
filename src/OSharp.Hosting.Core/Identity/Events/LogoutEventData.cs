// -----------------------------------------------------------------------
//  <copyright file="LogoutEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using OSharp.EventBuses;


namespace OSharp.Hosting.Identity.Events
{
    /// <summary>
    /// 用户退出事件数据
    /// </summary>
    public class LogoutEventData : EventDataBase
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public int UserId { get; set; }
    }
}