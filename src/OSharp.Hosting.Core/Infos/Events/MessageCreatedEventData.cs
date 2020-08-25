// -----------------------------------------------------------------------
//  <copyright file="MessageCreatedEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-15 9:27</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Infos.Entities;

using OSharp.EventBuses;


namespace OSharp.Hosting.Infos.Events
{
    /// <summary>
    /// 事件数据：发送消息
    /// </summary>
    public class MessageCreatedEventData : EventDataBase
    {
        /// <summary>
        /// 获取或设置 新增的消息
        /// </summary>
        public Message[] Messages { get; set; }
    }
}