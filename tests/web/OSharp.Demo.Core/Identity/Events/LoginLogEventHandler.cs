// -----------------------------------------------------------------------
//  <copyright file="LoginLogEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-30 0:41</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Demo.Identity.Entities;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.EventBuses;


namespace OSharp.Demo.Identity.Events
{
    public class LoginLogEventHandler : EventHandlerBase<LoginEventData>, ITransientDependency
    {
        private readonly IRepository<LoginLog, Guid> _loginLogRepository;

        /// <summary>
        /// 初始化一个<see cref="LoginLogEventHandler"/>类型的新实例
        /// </summary>
        public LoginLogEventHandler(IRepository<LoginLog, Guid> loginLogRepository)
        {
            _loginLogRepository = loginLogRepository;
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(LoginEventData eventData)
        {
            LoginLog log = new LoginLog()
            {
                Ip = eventData.LoginDto.Ip,
                UserAgent = eventData.LoginDto.UserAgent,
                UserId = eventData.User.Id
            };
            _loginLogRepository.Insert(log);
        }

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns>是否成功</returns>
        public override Task HandleAsync(LoginEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            throw new NotSupportedException("日志记录不支持异步");
        }
    }
}