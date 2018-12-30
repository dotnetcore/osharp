// -----------------------------------------------------------------------
//  <copyright file="FireAndForgetJobBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 21:13</last-date>
// -----------------------------------------------------------------------

using System;

using Hangfire;


namespace OSharp.Hangfire
{
    /// <summary>
    /// 触发并遗忘作业基类
    /// </summary>
    public abstract class FireAndForgetJobBase : IFireAndForgetJob
    {
        /// <summary>
        /// 设置 继续作业对象
        /// </summary>
        protected virtual IContinuationJob ContinuationJob { private get; set; }

        /// <summary>
        /// 执行 触发并遗忘作业
        /// </summary>
        /// <returns>作业编号</returns>
        public string Execute()
        {
            string jobId = BackgroundJob.Enqueue(() => ExecuteAction());
            if (ContinuationJob != null)
            {
                ContinuationJob.Execute(jobId);
            }
            return jobId;
        }

        /// <summary>
        /// 重写以实现 触发并遗忘作业 委托
        /// </summary>
        public abstract object ExecuteAction();
    }
}