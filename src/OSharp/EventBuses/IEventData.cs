// -----------------------------------------------------------------------
//  <copyright file="IEventData.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-18 10:22</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 定义事件源数据，所有事件源都要实现该接口
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 获取 事件发生时间
        /// </summary>
        DateTime EventTime { get; }

        /// <summary>
        /// 获取或设置 触发事件的对象
        /// </summary>
        object EventSource { get; set; }
    }
}