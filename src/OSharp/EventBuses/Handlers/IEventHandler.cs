// -----------------------------------------------------------------------
//  <copyright file="IEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-18 10:25</last-date>
// -----------------------------------------------------------------------

using OSharp.Dependency;


namespace OSharp.EventBuses
{
    /// <summary>
    /// 定义事件处理器，所有事件处理都要实现该接口
    /// </summary>
    [IgnoreDependency]
    public interface IEventHandler : ITransientDependency
    { }


    /// <summary>
    /// 定义泛型事件处理器
    /// </summary>
    /// <typeparam name="TEventData">事件源数据</typeparam>
    [IgnoreDependency]
    public interface IEventHandler<in TEventData> : IEventHandler where TEventData : IEventData
    {
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        void HandleEvent(TEventData eventData);
    }
}