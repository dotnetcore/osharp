// -----------------------------------------------------------------------
//  <copyright file="DelayedJobBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 21:04</last-date>
// -----------------------------------------------------------------------

using System;

using Hangfire;


namespace OSharp.Hangfire
{
    /// <summary>
    /// 延迟作业基类
    /// </summary>
    public abstract class DelayedJobBase : IDelayedJob
    {
        /// <summary>
        /// 获取或设置 延迟时间间隔
        /// </summary>
        public abstract TimeSpan Delay { get; }

        /// <summary>
        /// 设置 继续作业对象
        /// </summary>
        protected virtual IContinuationJob ContinuationJob { private get; set; }

        /// <summary>
        /// 执行延迟作业
        /// </summary>
        /// <returns>作业编号</returns>
        public string Execute()
        {
            string jobId = BackgroundJob.Schedule(() => ExecuteAction(), Delay);
            if (ContinuationJob != null)
            {
                ContinuationJob.Execute(jobId);
            }
            return jobId;
        }

        /// <summary>
        /// 重写以实现延迟作业委托
        /// </summary>
        public abstract object ExecuteAction();
    }
}