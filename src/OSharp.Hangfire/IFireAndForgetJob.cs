// -----------------------------------------------------------------------
//  <copyright file="IFireAndForgetHangfireJob.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-30 17:08</last-date>
// -----------------------------------------------------------------------

using OSharp.Dependency;


namespace OSharp.Hangfire
{
    /// <summary>
    /// 触发并遗忘作业，只执行一次，并且几乎在创建后立即执行。
    /// </summary>
    [MultipleDependency]
    public interface IFireAndForgetJob
    {
        /// <summary>
        /// 执行 触发并遗忘作业
        /// </summary>
        /// <returns>作业编号</returns>
        string Execute();
    }
}