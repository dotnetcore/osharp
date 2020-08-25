// -----------------------------------------------------------------------
//  <copyright file="Login_LoginLogEventHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

using OSharp.Hosting.Identity.Entities;

using OSharp.Entity;
using OSharp.EventBuses;


namespace OSharp.Hosting.Identity.Events
{
    /// <summary>
    /// 用户登录事件：登录日志
    /// </summary>
    public class LoginLoginLogEventHandler : EventHandlerBase<LoginEventData>
    {
        private readonly IRepository<LoginLog, Guid> _loginLogRepository;

        /// <summary>
        /// 初始化一个<see cref="LoginLoginLogEventHandler"/>类型的新实例
        /// </summary>
        public LoginLoginLogEventHandler(IRepository<LoginLog, Guid> loginLogRepository)
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
            LoginLog log = new LoginLog()
            {
                Ip = eventData.LoginDto.Ip,
                UserAgent = eventData.LoginDto.UserAgent,
                UserId = eventData.User.Id
            };
            return _loginLogRepository.InsertAsync(log);
        }
    }
}