// -----------------------------------------------------------------------
//  <copyright file="IDelayedHangfireJob.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 17:10</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Dependency;


namespace OSharp.Hangfire
{
    /// <summary>
    /// 延迟作业，只执行一次，并且在一定时间间隔后才执行。
    /// </summary>
    [MultipleDependency]
    public interface IDelayedJob
    {
        /// <summary>
        /// 获取 延迟时间间隔
        /// </summary>
        TimeSpan Delay { get; }

        /// <summary>
        /// 执行延迟作业
        /// </summary>
        /// <returns>作业编号</returns>
        string Execute();
    }
}