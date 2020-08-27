// -----------------------------------------------------------------------
//  <copyright file="MessageCreatedEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-15 9:27</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Hosting.Infos.Dtos;

using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.EventBuses;


namespace OSharp.Hosting.Infos.Events
{
    /// <summary>
    /// 事件处理器：发送消息
    /// </summary>
    public class MessageCreatedEventHandler : EventHandlerBase<MessageCreatedEventData>
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="MessageCreatedEventHandler"/>类型的新实例
        /// </summary>
        public MessageCreatedEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(MessageCreatedEventHandler));
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(MessageCreatedEventData eventData)
        {
            throw new NotSupportedException("发送消息事件处理器不支持同步处理");
        }

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns>是否成功</returns>
        public override Task HandleAsync(MessageCreatedEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            _logger.LogInformation(0, eventData.Messages.Select(m => new MessageOutputDto(m)).ExpandAndToString());
            return Task.CompletedTask;
        }
    }
}