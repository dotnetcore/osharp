// -----------------------------------------------------------------------
//  <copyright file="HangfireJobRunner.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-31 17:36</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Hangfire;

using Liuliu.Demo.Identity;
using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Collections;
using OSharp.Dependency;
using OSharp.Hangfire;


namespace Liuliu.Demo.Web.Hangfire
{
    [Dependency(ServiceLifetime.Singleton)]
    public class HangfireJobRunner : IHangfireJobRunner
    {
        public void Start()
        {
            BackgroundJob.Enqueue<UserManager<User>>(m => m.FindByIdAsync("1"));
            string jobId = BackgroundJob.Schedule<UserManager<User>>(m => m.FindByIdAsync("2"), TimeSpan.FromMinutes(2));
            BackgroundJob.ContinueWith<TestHangfireJob>(jobId, m => m.GetUserCount());
            RecurringJob.AddOrUpdate<TestHangfireJob>(m => m.GetUserCount(), Cron.Minutely, TimeZoneInfo.Local);
        }
    }


    public class TestHangfireJob
    {
        private readonly IIdentityContract _identityContract;

        /// <summary>
        /// 初始化一个<see cref="TestHangfireJob"/>类型的新实例
        /// </summary>
        public TestHangfireJob(IIdentityContract identityContract)
        {
            _identityContract = identityContract;
        }

        /// <summary>
        /// 获取用户数量
        /// </summary>
        public string GetUserCount()
        {
            List<string> list = new List<string>();
            list.Add(_identityContract.Users.Count().ToString());
            list.Add(_identityContract.GetHashCode().ToString());
            return list.ExpandAndToString();
        }
    }
}