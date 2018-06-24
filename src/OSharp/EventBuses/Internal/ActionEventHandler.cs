// -----------------------------------------------------------------------
//  <copyright file="ActionEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-18 10:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Data;


namespace OSharp.EventBuses.Internal
{
    /// <summary>
    /// 支持<see cref="Action"/>的事件处理器
    /// </summary>
    internal class ActionEventHandler<TEventData> : EventHandlerBase<TEventData> where TEventData : IEventData
    {
        /// <summary>
        /// 初始化一个<see cref="ActionEventHandler{TEventData}"/>类型的新实例
        /// </summary>
        public ActionEventHandler(Action<TEventData> action)
        {
            Action = action;
        }

        /// <summary>
        /// 获取 事件执行的委托
        /// </summary>
        public Action<TEventData> Action { get; }
        
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(TEventData eventData)
        {
            Check.NotNull(eventData, nameof(eventData));
            Action(eventData);
        }

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns>是否成功</returns>
        public override Task HandleAsync(TEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            Check.NotNull(eventData, nameof(eventData));
            cancelToken.ThrowIfCancellationRequested();
            return Task.Run(() => Action(eventData), cancelToken);
        }
    }
}