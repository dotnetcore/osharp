// -----------------------------------------------------------------------
//  <copyright file="IEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-01-12 11:43</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 定义事件数据，所有事件都要实现该接口
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 获取 事件编号
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 获取 事件发生的时间
        /// </summary>
        DateTime EventTime { get; }

        /// <summary>
        /// 获取或设置 事件源，触发事件的对象
        /// </summary>
        object EventSource { get; set; }
    }
}