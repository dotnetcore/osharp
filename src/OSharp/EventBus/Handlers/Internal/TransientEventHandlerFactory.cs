// -----------------------------------------------------------------------
//  <copyright file="TransientEventHandlerFactory.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-19 1:31</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.EventBus.Handlers.Internal
{
    /// <summary>
    /// 即时生命周期的事件处理器实例获取方式
    /// </summary>
    internal class TransientEventHandlerFactory<TEventHandler> : IEventHandlerFactory
        where TEventHandler : IEventHandler, new()
    {
        /// <summary>
        /// 获取事件处理器实例
        /// </summary>
        /// <returns></returns>
        public IEventHandler GetHandler()
        {
            return new TEventHandler();
        }

        /// <summary>
        /// 释放事件处理器实例
        /// </summary>
        /// <param name="handler"></param>
        public void ReleaseHandler(IEventHandler handler)
        {
            (handler as IDisposable)?.Dispose();
        }
    }
}