// -----------------------------------------------------------------------
//  <copyright file="RecurringJobBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 21:08</last-date>
// -----------------------------------------------------------------------

using System;

using Hangfire;


namespace OSharp.Hangfire
{
    /// <summary>
    /// 重复作业基类
    /// </summary>
    public abstract class RecurringJobBase : IRecurringJob
    {
        /// <summary>
        /// 获取或设置 重复执行时间的CRON表达式
        /// </summary>
        public abstract string CronExpression { get; }

        /// <summary>
        /// 执行重复作业
        /// </summary>
        public void Execute()
        {
            RecurringJob.AddOrUpdate(() => ExecuteAction(), CronExpression, TimeZoneInfo.Local);
        }

        /// <summary>
        /// 重写以实现重复作业委托
        /// </summary>
        public abstract object ExecuteAction();
    }
}