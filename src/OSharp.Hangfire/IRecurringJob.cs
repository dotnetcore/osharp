// -----------------------------------------------------------------------
//  <copyright file="IRecurringHangfireJob.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 17:12</last-date>
// -----------------------------------------------------------------------

using OSharp.Dependency;


namespace OSharp.Hangfire
{
    /// <summary>
    /// 重复作业按指定的cron计划多次触发。
    /// </summary>
    [MultipleDependency]
    public interface IRecurringJob
    {
        /// <summary>
        /// 获取 重复执行时间的CRON表达式
        /// </summary>
        string CronExpression { get; }
        
        /// <summary>
        /// 执行重复作业
        /// </summary>
        void Execute();
    }
}