// -----------------------------------------------------------------------
//  <copyright file="IContinuationJob.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 21:37</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hangfire
{
    /// <summary>
    /// 定义当父作业完成后，将执行连续操作的作业
    /// </summary>
    public interface IContinuationJob
    {
        /// <summary>
        /// 继续执行的作业
        /// </summary>
        /// <param name="parentJobId">父作业编号</param>
        void Execute(string parentJobId);
    }
}