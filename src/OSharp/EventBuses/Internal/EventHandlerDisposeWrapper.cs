// -----------------------------------------------------------------------
//  <copyright file="EventHandlerDisposeWrapper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-21 1:12</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.EventBuses.Internal
{
    /// <summary>
    /// <see cref="IEventHandler"/>事件处理器的可释放包装
    /// </summary>
    public class EventHandlerDisposeWrapper : Disposable
    {
        private readonly Action _disposeAction;

        /// <summary>
        /// 初始化一个<see cref="EventHandlerDisposeWrapper"/>类型的新实例
        /// </summary>
        public EventHandlerDisposeWrapper(IEventHandler eventHandler, Action disposeAction = null)
        {
            _disposeAction = disposeAction;
            EventHandler = eventHandler;
        }

        /// <summary>
        /// 获取或设置 事件处理器对象
        /// </summary>
        public IEventHandler EventHandler { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                _disposeAction?.Invoke();
            }
            base.Dispose(disposing);
        }
    }
}