// -----------------------------------------------------------------------
//  <copyright file="ContinuationJobBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 21:37</last-date>
// -----------------------------------------------------------------------

using Hangfire;


namespace OSharp.Hangfire
{
    /// <summary>
    /// 继续执行作业的基类
    /// </summary>
    public abstract class ContinuationJobBase : IContinuationJob
    {
        /// <summary>
        /// 继续执行的作业
        /// </summary>
        /// <param name="parentJobId">父作业编号</param>
        public void Execute(string parentJobId)
        {
            BackgroundJob.ContinueWith(parentJobId, () => ExecuteAction());
        }

        /// <summary>
        /// 重写以实现重复作业委托
        /// </summary>
        public abstract object ExecuteAction();
    }
}