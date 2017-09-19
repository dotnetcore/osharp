// -----------------------------------------------------------------------
//  <copyright file="IocEventHandlerFactory.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-19 3:56</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.EventBuses.Handlers
{
    /// <summary>
    /// 依赖注入事件处理器实例获取方式
    /// </summary>
    public class IocEventHandlerFactory : IEventHandlerFactory
    {
        private readonly Type _handlerType;

        /// <summary>
        /// 初始化一个<see cref="IocEventHandlerFactory"/>类型的新实例
        /// </summary>
        /// <param name="handlerType">事件处理器类型</param>
        public IocEventHandlerFactory(Type handlerType)
        {
            _handlerType = handlerType;
        }

        /// <summary>
        /// 获取事件处理器实例
        /// </summary>
        /// <returns></returns>
        public IEventHandler GetHandler()
        {
            return ServiceLocator.Instance.GetService(_handlerType) as IEventHandler;
        }

        /// <summary>
        /// 释放事件处理器实例
        /// </summary>
        /// <param name="handler"></param>
        public void ReleaseHandler(IEventHandler handler)
        { }
    }
}